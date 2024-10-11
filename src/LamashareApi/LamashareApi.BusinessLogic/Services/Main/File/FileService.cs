using System.Reflection;
using Lamashare.BusinessLogic.Dtos.File;
using Lamashare.BusinessLogic.Dtos.Generic;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Enums;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Constants;
using LamashareApi.Shared.Exceptions.UserspaceException;
using LamashareCore.Models;
using LamashareCore.Util;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using Block = LamashareCore.Models.Block;
using BlockDto = Lamashare.BusinessLogic.Dtos.File.BlockDto;

namespace Lamashare.BusinessLogic.Services.Main.File;

public class FileService(IRepoWrapper repoWrap, IAppsettingsProvider apps) : IFileService
{
    #region GET - Total checksum
    public async Task<FileChecksumDto> GetTotalChecksum(Guid libraryId, string libraryFilePath)
    {
        #region Load library
        LamashareApi.Database.DB.Entities.Library lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        #endregion
        #region Generate checksum

        string libSysPath = LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation);
        string fileSysPath = FileUtil.FileLibPathToSysPath(libSysPath, libraryFilePath);
        string check = await FileUtil.GetFileTotalChecksumAsync(fileSysPath);
        #endregion
        
        return new FileChecksumDto()
        {
            Checksum = check,
        };
    }
    #endregion
    #region GET - File info
    public async Task<FileInfoDto> GetFileInfo(Guid libraryId, string libraryFilePath)
    {
        #region load lib and file
        LamashareApi.Database.DB.Entities.Library lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        LamashareApi.Database.DB.Entities.File? file = await repoWrap.FileRepo.QueryAll().FirstOrDefaultAsync(x => x.FileLibraryPath == libraryFilePath);
        if (file == null) throw new NotFoundUSException();
        #endregion
        #region generate info

        string fileSysPath =
            Path.Combine(LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation), libraryFilePath);
        FileInfo fileInfo = new FileInfo(fileSysPath);
        if (!fileInfo.Exists)
        {
            throw new NotFoundUSException();
        }

        var infoDto = new FileInfoDto()
        {
            LibraryId = libraryId,
            DateModified = file.DateModified.UtcDateTime,
            DateCreated = file.DateCreated.UtcDateTime,
            TotalChecksum = await FileUtil.GetFileTotalChecksumAsync(fileSysPath),
            FileLibraryPath = libraryFilePath,
        };
        #endregion

        return infoDto;
    }
    #endregion
    #region GET - File status
    public async Task<FileStatusDto> GetFileStatus(Guid libraryId, string libraryFilePath)
    {
        #region libs load
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        #endregion

        throw new NotImplementedException();
    }
    #endregion
    #region GET - File blocklist
    public async Task<string[]> GetFileBlockList(Guid libraryId, string libraryFilePath)
    {
        #region load lib
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        #endregion
        #region generate blocklist
        string syspath = Path.Combine(LibraryUtil.GetLibraryDirectory(libraryId, apps.GetAppsettings().Storage.LibraryLocation), libraryFilePath);
        var blocks = FileUtil.GetFileBlocks(syspath);
        #endregion

        return blocks.Select(x => x.Checksum).ToArray();
    }
    #endregion
    #region GET - Pull block
    public async Task<BlockDto> PullBlock(string blockChecksum)
    {
        #region load block
        var blockEntity = await repoWrap.BlockRepo
            .QueryAll()
            .Include(x => x.File)
            .ThenInclude(x => x.Library)
            .FirstOrDefaultAsync(x => x.Checksum == blockChecksum);
        if (blockEntity == null || blockEntity.File == null || blockEntity.File.Library == null)
        {
            throw new NotFoundUSException();
        }
        #endregion
        #region Load block from file
        string syspath = Path.Join(LibraryUtil.GetLibraryDirectory(blockEntity.File.Library.Id, apps.GetAppsettings().Storage.LibraryLocation), blockEntity.File.FileLibraryPath);
        byte[] block = await FileUtil.GetFileBlock(blockChecksum, syspath, blockEntity.Size, blockEntity.Offset);
        #endregion

        return new()
        {
            Checksum = blockChecksum,
            Content = block
        };
    }
    #endregion
    #region POST - Create transaction
    public async Task<FileTransactionDto> CreateFileTransaction(FileTransactionCreateDto createDto)
    {
        #region load file
        bool hasFileJustBeenCreated = false;
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(createDto.LibraryId);
        var existingFile = await repoWrap.FileRepo.QueryAll()
            .Include(x => x.Blocks)
            .FirstOrDefaultAsync(x => x.FileLibraryPath == createDto.FileLibraryPath && x.Library == lib);

        if (existingFile == null && createDto.Type == EFileTransactionType.PULL)
        {
            throw new NotFoundUSException();
        }

        if (existingFile == null && createDto.Type == EFileTransactionType.PUSH)
        {
            hasFileJustBeenCreated = true;
            existingFile = await repoWrap.FileRepo.InsertAsync(new()
            {
                FileLibraryPath = createDto.FileLibraryPath,
                Library = lib,
                TotalChecksum = createDto.TotalChecksum!,
                DateModified = createDto.DateModifiedFile,
                DateCreated = createDto.DateCreatedFile,
            });
        }

        // Check again if file has really been created
        if (existingFile == null)
        {
            throw new ArgumentException("Failed to get file.");
        }
        #endregion
        #region check if already locked
        if (await IsFileLocked(existingFile.Id))
        {
            throw new TransactionConflictUSException();
        }
        #endregion
        
        #region create transaction db entity
        var creationDateTime = DateTime.UtcNow;
        var newTransaction = new FileTransaction()
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
            RequiredBlocks = createDto.Type == EFileTransactionType.PUSH ? createDto.BlockChecksums!.ToList() : null,
        };
        existingFile.FileTransactions.Add(newTransaction);
        await repoWrap.FileTransactionRepo.InsertAsync(newTransaction);
        await repoWrap.FileRepo.UpdateAsync(existingFile);
        
        #endregion
        #region prepare temp blocks
        List<string> blocksRequiredByRemoteOnPush = new();
        if (createDto.Type == EFileTransactionType.PULL)
        {
            var blocks = FileUtil.GetFileBlocks(Path.Join(LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation), existingFile.FileLibraryPath));
            await WriteTempBlockRange(blocks);
        }
        else
        {
            var existingBlocks = await repoWrap.BlockRepo.QueryAll()
                .Include(x => x.File)
                .Where(x => createDto.BlockChecksums.Contains(x.Checksum) && !(hasFileJustBeenCreated && x.File == existingFile))
                .ToListAsync();

            foreach (var block in existingBlocks)
            {
                string libSysPath = LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation);
                string fileSysPath = FileUtil.FileLibPathToSysPath(libSysPath, block.File.FileLibraryPath);
                var blockContent = await FileUtil.GetFileBlock(block.Checksum, fileSysPath, block.Size, block.Offset);
                await WriteTempBlock(block.Checksum, blockContent);
            }

            foreach (var blockOnClient in createDto.BlockChecksums)
            {
                if (!existingBlocks.Select(x => x.Checksum).Contains(blockOnClient))
                {
                    blocksRequiredByRemoteOnPush.Add(blockOnClient);
                }
            }
            
            newTransaction.RequiredBlocks = blocksRequiredByRemoteOnPush;
            await repoWrap.FileTransactionRepo.UpdateAsync(newTransaction);
        }
        #endregion
        
        return new()
        {
            FileId = existingFile.Id,
            Id = newTransaction.Id,
            Type = createDto.Type,
            RequiredBlocks = blocksRequiredByRemoteOnPush,
        };
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
            .FirstOrDefaultAsync(x => x.Id == transactionId);
        if (transaction == null)
        {
            throw new NotFoundUSException();
        }
        
        if (transaction.Status == EFileTransactionStatus.COMPLETE)
        {
            throw new TransactionAlreadyCompleteUSException();
        }

        var file = transaction.File;
        #endregion 
        
        #region checks
        if (transaction.Type == EFileTransactionType.PUSH 
            && (transaction.RequiredBlocks!.Count != transaction.ReceivedBlocks!.Count || transaction.RequiredBlocks.Except(transaction.ReceivedBlocks).Any()))
        {
            throw new TransactionBlockMismatchUSException();
        }

        string libpath = LibraryUtil.GetLibraryDirectory(transaction.File.Library.Id,
            apps.GetAppsettings().Storage.LibraryLocation);
        #endregion
        
        #region finalize push transaction specific stuff
        if (transaction.Type == EFileTransactionType.PUSH)
        {
            string fileSysPath = Path.Join(libpath, file.FileLibraryPath);
            #region write combined blocks to disk
            // Skip combination if no blocks were expected (empty file creation)
            if (transaction.TotalBlocks.Any())
            {
                await CombineAndDiscardTempBlocks(transaction.File.Library.Id, fileSysPath, transaction.TotalBlocks!, transaction.File.DateModified, transaction.File.DateCreated);
            }
            else
            {
                await FileUtil.FullRestoreFileFromBlocks(new(), fileSysPath, transaction.File.DateCreated,
                    transaction.File.DateModified);
            }
            #endregion
            
            
            string assembledFileChecksum = await FileUtil.GetFileTotalChecksumAsync(fileSysPath);
            //if (transaction.ExpectedChecksum != assembledFileChecksum)
            //{
            //    throw new TransactionChecksumMismatchUSException();
            //}
        
            #region Update file db entity
            file.TotalChecksum = assembledFileChecksum;
            file.DateCreated = transaction.DateCreatedFile;
            file.DateModified = transaction.DateModifiedFile;
            await repoWrap.FileRepo.UpdateAsync(file);
            #endregion
        }
        #endregion
        
        #region update transaction db entity
        transaction.Status = EFileTransactionStatus.COMPLETE;
        await repoWrap.FileTransactionRepo.UpdateAsync(transaction);
        #endregion

        return new()
        {
            Success = true
        };
    }
    #endregion
    #region POST - Push block
    public async Task<StatusDto> PushBlock(BlockPushDto blockDto)
    {
        #region load transaction
        var transaction = await repoWrap.FileTransactionRepo
            .QueryAll()
            .Include(x => x.File)
            .FirstOrDefaultAsync(x => x.Id == blockDto.TransactionId);
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(blockDto.LibraryId);
        if (transaction == null)
        {
            throw new NotFoundUSException();
        }
        
        if (transaction.Type == EFileTransactionType.PULL)
        {
            throw new TransactionTypeUSException();
        }
        
        if (transaction.ReceivedBlocks == null)
        {
            throw new ArgumentException("Received blocks are null");
        }
        #endregion

        await WriteTempBlock(blockDto.Checksum, blockDto.Content);
        
        await repoWrap.BlockRepo.InsertAsync(new()
        {
            File = transaction.File,
            Checksum = blockDto.Checksum,
            Library = lib,
            Offset = blockDto.Offset,
            Size = blockDto.Size,
        });
        transaction.ReceivedBlocks.Add(blockDto.Checksum);
        await repoWrap.FileTransactionRepo.UpdateAsync(transaction);

        return new StatusDto()
        {
            Ok = true,
        };
    }
    #endregion
    #region POST - Create diff
    public async Task<LibraryDiffDto> CreateLibraryDiff(CreateDiffDto dto)
    {
        #region load lib
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(dto.LibraryId);
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
            int tolerance = 1000;
            if (Math.Abs((localFileMatch.DateModified.UtcDateTime - file.DateModified.UtcDateTime).Duration().TotalMilliseconds) <
                tolerance)
            {
                // Files are in sync, ignore
                continue;
            }
            
            if (localFileMatch.DateModified.UtcDateTime < file.DateModified.UtcDateTime)
            {
                missingOnLocal.Add(file.FileLibraryPath);
            }
            else
            {
                missingOnRemote.Add(file.FileLibraryPath);
            }
        }

        foreach (var file in dto.FilesOnLocal)
        {
            var remoteFileMatch = filesInRemoteLib.FirstOrDefault(x => x.FileLibraryPath == file.FileLibraryPath);
            if (remoteFileMatch == null)
            {
                missingOnRemote.Add(file.FileLibraryPath);
            }
        }
        #endregion

        return new()
        {
            RequiredByLocal = missingOnLocal,
            RequiredByRemote = missingOnRemote,
        };
    }
    #endregion
    #region DELETE - Delete file
    public async Task DeleteFile(Guid libraryId, string libraryFilePath)
    {
        #region load required data
        LamashareApi.Database.DB.Entities.Library lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        string libPath = LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation);
        string fileSysPath = FileUtil.FileLibPathToSysPath(libPath, libraryFilePath);
        #endregion

        #region Delete file from FS
        if (global::System.IO.File.Exists(fileSysPath))
        {
            global::System.IO.File.Delete(fileSysPath);
        }
        else
        {
            throw new NotFoundUSException();
        }
        #endregion
        #region Delete file from db
        LamashareApi.Database.DB.Entities.File dbFile = await repoWrap
            .FileRepo
            .QueryAll()
            .Include(x => x.Blocks)
            .FirstAsync(x => x.Library == lib && x.FileLibraryPath == libraryFilePath);

        List<string> blockChecksums = dbFile.Blocks.Select(x => x.Checksum).ToList();
        #endregion
        
        #region Delete blocks from db
        List<LamashareApi.Database.DB.Entities.Block> toDelete = await repoWrap.BlockRepo
            .QueryAll()
            .Include(x => x.File)
            .Where(x => x.File == dbFile)
            .ToListAsync();

        await repoWrap.BlockRepo.DeleteRangeAsync(toDelete);
        #endregion
    }
    #endregion
    
    #region Util

    private async Task WriteTempBlockRange(List<Block> blocks)
    {
        foreach (var block in blocks)
        {
            await WriteTempBlock(block.Checksum, block.Payload);
        }
    }
    
    private async Task WriteTempBlock(string checksum, byte[] bytes)
    {
        string targetPath = Path.Join(apps.GetAppsettings().Storage.TempBlockLocation, checksum);
        if (!Directory.Exists(apps.GetAppsettings().Storage.TempBlockLocation))
        {
            Directory.CreateDirectory(apps.GetAppsettings().Storage.TempBlockLocation);
        }
        
        using (FileStream fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.WriteAsync(bytes, 0, bytes.Length);
        }
    }

    private async Task CombineAndDiscardTempBlocks(Guid libId, string outputSysPath, List<string> checksums, DateTimeOffset createdOn, DateTimeOffset modifiedOn)
    {
        string c = apps.GetAppsettings().Storage.TempBlockLocation;
        
        await FileUtil.FullRestoreFileFromBlocks(checksums.Select(x => Path.Join(c, x)).ToList(), outputSysPath, createdOn, modifiedOn);
    }

    private async Task<bool> IsFileLocked(Guid fileId)
    {
        var file = await repoWrap.FileRepo
            .QueryAll()
            .Include(x => x.FileTransactions)
            .FirstOrDefaultAsync(x => x.Id == fileId);

        if (file == null) return false;
        
        var ongoingTransaction = file.FileTransactions.FirstOrDefault(x => x.Status == EFileTransactionStatus.INCOMPLETE);
        return ongoingTransaction != null;
    }
    #endregion
}