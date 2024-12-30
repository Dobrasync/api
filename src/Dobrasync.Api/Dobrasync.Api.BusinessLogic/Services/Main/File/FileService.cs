using Dobrasync.Api.BusinessLogic.Dtos.File;
using Dobrasync.Api.BusinessLogic.Dtos.Generic;
using Dobrasync.Api.BusinessLogic.Services.Core.AccessControl;
using Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;
using Dobrasync.Api.Database.DB.Entities;
using Dobrasync.Api.Database.Enums;
using Dobrasync.Api.Database.Repos;
using Dobrasync.Api.Shared.Constants;
using Dobrasync.Api.Shared.Exceptions.UserspaceException;
using Dobrasync.Api.Shared.Exceptions.UserspaceException.Block;
using Dobrasync.Api.Shared.Exceptions.UserspaceException.Transaction;
using Dobrasync.Core.Common.Models;
using Dobrasync.Core.Common.Util;
using Microsoft.EntityFrameworkCore;
using BlockDto = Dobrasync.Api.BusinessLogic.Dtos.File.BlockDto;

namespace Dobrasync.Api.BusinessLogic.Services.Main.File;

public class FileService(IRepoWrapper repoWrap, IAppsettingsProvider apps, IAccessControlService acs) : IFileService
{
    #region GET - Total checksum

    public async Task<FileChecksumDto> GetTotalChecksum(Guid libraryId, string libraryFilePath)
    {
        #region Load library

        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);

        #endregion

        #region Access-Control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion
        
        #region Generate checksum

        var libSysPath = LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation);
        var fileSysPath = FileUtil.FileLibPathToSysPath(libSysPath, libraryFilePath);
        var check = await FileUtil.GetFileTotalChecksumAsync(fileSysPath);

        #endregion

        return new FileChecksumDto
        {
            Checksum = check
        };
    }

    #endregion

    #region GET - File info

    public async Task<FileInfoDto> GetFileInfo(Guid libraryId, string libraryFilePath)
    {
        #region load lib and file

        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        var file = await repoWrap.FileRepo.QueryAll().FirstOrDefaultAsync(x => x.FileLibraryPath == libraryFilePath);
        if (file == null) throw new NotFoundUSException();

        #endregion
        
        #region Access-Control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion

        #region generate info

        var fileSysPath =
            Path.Combine(LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation),
                libraryFilePath);
        var fileInfo = new FileInfo(fileSysPath);
        if (!fileInfo.Exists) throw new NotFoundUSException();

        var infoDto = new FileInfoDto
        {
            LibraryId = libraryId,
            DateModified = file.DateModified.UtcDateTime,
            DateCreated = file.DateCreated.UtcDateTime,
            TotalChecksum = await FileUtil.GetFileTotalChecksumAsync(fileSysPath),
            FileLibraryPath = libraryFilePath
        };

        #endregion

        return infoDto;
    }

    #endregion

    #region GET - File status

    public async Task<FileStatusDto> GetFileStatus(Guid libraryId, string libraryFilePath)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region GET - File blocklist

    public async Task<string[]> GetFileBlockList(Guid libraryId, string libraryFilePath)
    {
        #region load lib

        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);

        #endregion
        
        #region Access-Control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion

        #region generate blocklist

        var syspath =
            Path.Combine(LibraryUtil.GetLibraryDirectory(libraryId, apps.GetAppsettings().Storage.LibraryLocation),
                libraryFilePath);
        var blocks = FileUtil.GetFileBlocks(syspath);

        #endregion

        return blocks.Select(x => x.Checksum).ToArray();
    }

    #endregion

    #region GET - Pull block

    public async Task<BlockDto> PullBlock(string blockChecksum)
    {
        #region Load file containing block

        var file = await repoWrap.FileRepo
            .QueryAll()
            .Include(x => x.Blocks)
            .Include(x => x.Library)
            .FirstOrDefaultAsync(x => x.Blocks.Any(y => y.Checksum == blockChecksum));

        #region Access-Control

        if (file != null)
        {
            (await acs.FromInvoker()).OwnsLibrary(file.Library);
        }
        
        #endregion
        
        if (file == null) throw new NotFoundUSException();

        var blockEntity = file.Blocks.First(x => x.Checksum == blockChecksum);

        #endregion

        #region Load block content from file

        var syspath =
            Path.Join(LibraryUtil.GetLibraryDirectory(file.Library.Id, apps.GetAppsettings().Storage.LibraryLocation),
                file.FileLibraryPath);
        var block = await FileUtil.GetFileBlock(blockChecksum, syspath, blockEntity.Size, blockEntity.Offset);

        #endregion

        return new BlockDto
        {
            Checksum = blockChecksum,
            Content = block
        };
    }

    #endregion

    #region POST - Create transaction

    public async Task<FileTransactionDto> CreateFileTransaction(FileTransactionCreateDto createDto)
    {
        #region load lib

        var hasFileJustBeenCreated = false;
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(createDto.LibraryId);
        
        #endregion
 
        #region Access-Control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion
        
        #region Load file
        var existingFile = await repoWrap.FileRepo.QueryAll()
            .Include(x => x.Blocks)
            .FirstOrDefaultAsync(x => x.FileLibraryPath == createDto.FileLibraryPath && x.Library == lib);

        if (existingFile == null && createDto.Type == EFileTransactionType.PULL) throw new NotFoundUSException();

        if (existingFile == null && createDto.Type == EFileTransactionType.PUSH)
        {
            hasFileJustBeenCreated = true;
            existingFile = await repoWrap.FileRepo.InsertAsync(new FileEntity
            {
                FileLibraryPath = createDto.FileLibraryPath,
                Library = lib,
                TotalChecksum = createDto.TotalChecksum!,
                DateModified = createDto.DateModifiedFile,
                DateCreated = createDto.DateCreatedFile
            });
        }

        // Check again if file has really been created
        if (existingFile == null) throw new ArgumentException("Failed to get file.");

        #endregion

        #region check if already locked

        if (await IsFileLocked(existingFile.Id)) throw new TransactionConflictUSException();

        #endregion

        #region create transaction db entity

        var creationDateTime = DateTime.UtcNow;
        var newTransaction = new FileTransactionEntity
        {
            File = existingFile,
            Status = EFileTransactionStatus.INCOMPLETE,
            DateCreated = creationDateTime,
            DateModified = creationDateTime,
            DateCreatedFile = createDto.DateCreatedFile,
            DateModifiedFile = createDto.DateModifiedFile,
            Type = createDto.Type,
            ExpectedChecksum = createDto.Type == EFileTransactionType.PUSH ? createDto.TotalChecksum : null,
            TotalBlocks = createDto.Type == EFileTransactionType.PUSH ? createDto.BlockChecksums!.ToList() : null,
            RequiredBlocks = createDto.Type == EFileTransactionType.PUSH ? createDto.BlockChecksums!.ToList() : null
        };
        existingFile.FileTransactions.Add(newTransaction);
        await repoWrap.FileTransactionRepo.InsertAsync(newTransaction);
        await repoWrap.FileRepo.UpdateAsync(existingFile);

        #endregion

        #region prepare temp blocks

        List<string> blocksRequiredByRemoteOnPush = new();
        if (createDto.Type == EFileTransactionType.PULL)
        {
            var blocks = FileUtil.GetFileBlocks(Path.Join(
                LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation),
                existingFile.FileLibraryPath));
            await WriteTempBlockRange(blocks);
        }
        else if (createDto.Type == EFileTransactionType.PUSH)
        {
            if (createDto.BlockChecksums == null) throw new ArgumentException("BlockChecksums cannot be null.");

            var filesContainingRequiredBlocks = await repoWrap.FileRepo
                .QueryAll()
                .Include(x => x.Blocks)
                .Where(x => !(hasFileJustBeenCreated && x == existingFile))
                .Where(x => x.Blocks.Any(y => createDto.BlockChecksums.Contains(y.Checksum)))
                .ToListAsync();

            var blocksOnly = filesContainingRequiredBlocks.SelectMany(x => x.Blocks).Distinct().ToList();
            var requiredBlocksOnly = blocksOnly.Where(x => createDto.BlockChecksums!.Contains(x.Checksum)).ToList();

            foreach (var block in requiredBlocksOnly)
            {
                var libSysPath = LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation);
                var fileSysPath = FileUtil.FileLibPathToSysPath(libSysPath, block.Files.First().FileLibraryPath);
                var blockContent = await FileUtil.GetFileBlock(block.Checksum, fileSysPath, block.Size, block.Offset);
                await WriteTempBlock(block.Checksum, blockContent);
            }

            foreach (var blockOnClient in createDto.BlockChecksums)
                if (!requiredBlocksOnly.Select(x => x.Checksum).Contains(blockOnClient))
                    blocksRequiredByRemoteOnPush.Add(blockOnClient);

            newTransaction.RequiredBlocks = blocksRequiredByRemoteOnPush;
            await repoWrap.FileTransactionRepo.UpdateAsync(newTransaction);
        }

        #endregion

        return new FileTransactionDto
        {
            FileId = existingFile.Id,
            Id = newTransaction.Id,
            Type = createDto.Type,
            RequiredBlocks = blocksRequiredByRemoteOnPush
        };
    }

    #endregion

    #region POST - Push block

    public async Task<StatusDto> PushBlock(BlockPushDto blockDto)
    {
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(blockDto.LibraryId);
        
        #region Access-Control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion
        
        #region load transaction
        
        var transaction = await repoWrap.FileTransactionRepo
            .QueryAll()
            .Include(x => x.File)
            .ThenInclude(x => x.Library)
            .FirstOrDefaultAsync(x => x.Id == blockDto.TransactionId);

        #endregion

        #region Check if block already exists

        var existingBlock = await repoWrap.BlockRepo
            .QueryAll()
            .FirstOrDefaultAsync(x => x.Checksum == blockDto.Checksum);

        if (existingBlock != null) throw new BlockPushDuplicateUSException();

        
        if (transaction == null) throw new NotFoundUSException();

        if (transaction.Type == EFileTransactionType.PULL) throw new TransactionTypeUSException();

        if (transaction.ReceivedBlocks == null) throw new ArgumentException("Received blocks are null");

        #endregion

        await WriteTempBlock(blockDto.Checksum, blockDto.Content);

        await repoWrap.BlockRepo.InsertAsync(new BlockEntity
        {
            Files = [transaction.File],
            Checksum = blockDto.Checksum,
            Library = lib,
            Offset = blockDto.Offset,
            Size = blockDto.Size
        });

        transaction.ReceivedBlocks.Add(blockDto.Checksum);
        transaction.DateModified = DateTimeOffset.UtcNow;
        await repoWrap.FileTransactionRepo.UpdateAsync(transaction);

        return new StatusDto
        {
            Ok = true
        };
    }

    #endregion

    #region POST - Create diff

    public async Task<LibraryDiffDto> CreateLibraryDiff(CreateDiffDto dto)
    {
        #region load lib

        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(dto.LibraryId);

        #endregion
        
        #region Access-Control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion

        #region gen diff

        List<string> missingOnRemote = new();
        List<string> missingOnLocal = new();

        var filesInRemoteLib = await repoWrap.FileRepo
            .QueryAll()
            .Where(x => x.Library == lib)
            .ToListAsync();
        LibraryDiffDto diff = new();
        foreach (var file in filesInRemoteLib)
        {
            var localFileMatch = dto.FilesOnLocal.FirstOrDefault(x => x.FileLibraryPath == file.FileLibraryPath);
            if (localFileMatch == null)
            {
                missingOnLocal.Add(file.FileLibraryPath);
                continue;
            }

            // Here we have a tolerance value as precision issues might return false positives on equal values.
            var tolerance = 1000;
            if (Math.Abs((localFileMatch.DateModified.UtcDateTime - file.DateModified.UtcDateTime).Duration()
                    .TotalMilliseconds) <
                tolerance)
                // Files are in sync, ignore
                continue;

            if (localFileMatch.DateModified.UtcDateTime < file.DateModified.UtcDateTime)
                missingOnLocal.Add(file.FileLibraryPath);
            else
                missingOnRemote.Add(file.FileLibraryPath);
        }

        foreach (var file in dto.FilesOnLocal)
        {
            var remoteFileMatch = filesInRemoteLib.FirstOrDefault(x => x.FileLibraryPath == file.FileLibraryPath);
            if (remoteFileMatch == null) missingOnRemote.Add(file.FileLibraryPath);
        }

        #endregion

        return new LibraryDiffDto
        {
            RequiredByLocal = missingOnLocal,
            RequiredByRemote = missingOnRemote
        };
    }

    #endregion

    #region DELETE - Delete file

    public async Task DeleteFile(Guid libraryId, string libraryFilePath)
    {
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        
        #region Access-Control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion
        
        #region load required data
        
        var libPath = LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation);
        var fileSysPath = FileUtil.FileLibPathToSysPath(libPath, libraryFilePath);

        #endregion

        #region Get file to delete from db

        var dbFileEntity = await repoWrap
            .FileRepo
            .QueryAll()
            .Include(x => x.Blocks)
            .ThenInclude(x => x.Files)
            .FirstAsync(x => x.Library == lib && x.FileLibraryPath == libraryFilePath);

        #endregion

        #region Delete orphan blocks from db

        // delete blocks from db that are only present in the just deleted file
        var blocksToDelete = dbFileEntity.Blocks
            .Where(x => x.Files.Count() <= 1)
            .ToList();
        await repoWrap.BlockRepo.DeleteRangeAsync(blocksToDelete);

        #endregion

        #region Update remaining blocks file list

        var blocksToUpdate = dbFileEntity.Blocks
            .Where(x => x.Files.Count() > 1)
            .ToList();

        blocksToUpdate.ForEach(x => x.Files.Remove(dbFileEntity));
        await repoWrap.BlockRepo.UpdateRangeAsync(blocksToUpdate);

        #endregion

        #region Drop file from db

        await repoWrap.FileRepo.DeleteAsync(dbFileEntity);

        #endregion

        #region Delete file from FS

        if (global::System.IO.File.Exists(fileSysPath))
            global::System.IO.File.Delete(fileSysPath);
        else
            throw new NotFoundUSException();

        #endregion
    }

    #endregion

    #region POST - Finish transaction

    public async Task<FileTransactionFinishDto> FinishFileTransaction(Guid transactionId)
    {
        #region load transaction

        var transaction = await repoWrap.FileTransactionRepo
            .QueryAll()
            .Include(x => x.File)
            .ThenInclude(x => x.Library)
            .Include(x => x.File)
            .ThenInclude(x => x.Blocks)
            .ThenInclude(x => x.Library)
            .FirstOrDefaultAsync(x => x.Id == transactionId);

        #endregion
        
        #region Access-Control
        if (transaction != null) {
            (await acs.FromInvoker()).OwnsLibrary(transaction.File.Library);
        }
        #endregion

        #region checks

        if (transaction == null) throw new NotFoundUSException();

        if (transaction.Status == EFileTransactionStatus.COMPLETE) throw new TransactionAlreadyCompleteUSException();
        if (transaction.Type == EFileTransactionType.PUSH
            && (transaction.RequiredBlocks!.Count != transaction.ReceivedBlocks!.Count ||
                transaction.RequiredBlocks.Except(transaction.ReceivedBlocks).Any()))
            throw new TransactionBlockMismatchUSException();

        #endregion

        #region Finalize PUSH

        if (transaction.Type == EFileTransactionType.PUSH) await FinalizePushTransaction(transaction);

        #endregion

        #region Update transaction db entity

        transaction.DateModified = DateTimeOffset.UtcNow;
        transaction.Status = EFileTransactionStatus.COMPLETE;
        await repoWrap.FileTransactionRepo.UpdateAsync(transaction);

        #endregion

        return new FileTransactionFinishDto
        {
            Success = true
        };
    }

    private async Task FinalizePushTransaction(FileTransactionEntity transaction)
    {
        #region vars

        var libpath = LibraryUtil.GetLibraryDirectory(transaction.File.Library.Id,
            apps.GetAppsettings().Storage.LibraryLocation);
        var file = transaction.File;
        var fileSysPath = Path.Join(libpath, file.FileLibraryPath);

        #endregion

        #region checks

        if (transaction.TotalBlocks == null) throw new ArgumentException("TotalBlocks must not be null.");

        #endregion

        #region Combine blocks / Create file on file system

        // Skip combination if no blocks were expected (empty file creation)
        if (transaction.TotalBlocks.Any())
            await CombineAndDiscardTempBlocks(transaction.File.Library.Id, fileSysPath, transaction.TotalBlocks!,
                transaction.File.DateModified, transaction.File.DateCreated);
        else
            await FileUtil.FullRestoreFileFromBlocks(new List<string>(), fileSysPath, transaction.File.DateCreated,
                transaction.File.DateModified);

        #endregion

        #region Update file db entity

        var assembledFileChecksum = await FileUtil.GetFileTotalChecksumAsync(fileSysPath);

        file.TotalChecksum = assembledFileChecksum;
        file.DateCreated = transaction.DateCreatedFile;
        file.DateModified = transaction.DateModifiedFile;
        await repoWrap.FileRepo.UpdateAsync(file);

        #endregion

        #region Update used blocks to include this file as source

        var blocksToUpdate = await repoWrap.BlockRepo
            .QueryAll()
            .Where(x => x.Library == transaction.File.Library && transaction.TotalBlocks.Contains(x.Checksum))
            .ToListAsync();

        blocksToUpdate.ForEach(x => x.Files.Add(file));
        await repoWrap.BlockRepo.UpdateRangeAsync(blocksToUpdate);

        #endregion
    }

    #endregion

    #region Util

    private async Task WriteTempBlockRange(List<FileBlock> blocks)
    {
        foreach (var block in blocks) await WriteTempBlock(block.Checksum, block.Payload);
    }

    private async Task WriteTempBlock(string checksum, byte[] bytes)
    {
        var targetPath = Path.Join(apps.GetAppsettings().Storage.TempBlockLocation, checksum);
        if (!Directory.Exists(apps.GetAppsettings().Storage.TempBlockLocation))
            Directory.CreateDirectory(apps.GetAppsettings().Storage.TempBlockLocation);

        using (var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.WriteAsync(bytes, 0, bytes.Length);
        }
    }

    private async Task CombineAndDiscardTempBlocks(Guid libId, string outputSysPath, List<string> checksums,
        DateTimeOffset createdOn, DateTimeOffset modifiedOn)
    {
        var c = apps.GetAppsettings().Storage.TempBlockLocation;

        await FileUtil.FullRestoreFileFromBlocks(checksums.Select(x => Path.Join(c, x)).ToList(), outputSysPath,
            createdOn, modifiedOn);
    }

    private async Task<bool> IsFileLocked(Guid fileId)
    {
        var file = await repoWrap.FileRepo
            .QueryAll()
            .Include(x => x.FileTransactions)
            .FirstOrDefaultAsync(x => x.Id == fileId);

        if (file == null) return false;

        var ongoingTransaction =
            file.FileTransactions.FirstOrDefault(x => x.Status == EFileTransactionStatus.INCOMPLETE);
        return ongoingTransaction != null;
    }

    #endregion
}