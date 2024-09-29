using Lamashare.BusinessLogic.Dtos.File;
using Lamashare.BusinessLogic.Dtos.Generic;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Enums;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Exceptions.UserspaceException;
using LamashareCore.Util;
using Microsoft.EntityFrameworkCore;

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
        string sysPath = FileUtil.FileLibPathToSysPath(apps.GetAppsettings().Storage.LibraryLocation, libraryFilePath);
        string check = await FileUtil.GetFileTotalChecksumAsync(sysPath);
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
        #region load lib
        LamashareApi.Database.DB.Entities.Library lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        #endregion
        #region generate info

        string fileSysPath =
            FileUtil.FileLibPathToSysPath(apps.GetAppsettings().Storage.LibraryLocation, libraryFilePath);
        FileInfo fileInfo = new FileInfo(fileSysPath);
        if (!fileInfo.Exists)
        {
            throw new NotFoundUSException();
        }

        var infoDto = new FileInfoDto()
        {
            LibraryId = libraryId,
            ModifiedOn = fileInfo.LastWriteTimeUtc,
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
        string syspath = FileUtil.FileLibPathToSysPath(apps.GetAppsettings().Storage.LibraryLocation, libraryFilePath);
        var blocks = FileUtil.GetFileBlocks(syspath);
        #endregion

        return blocks.Select(x => x.Checksum).ToArray();
    }
    #endregion
    #region GET - Pull block
    public async Task<BlockDto> PullBlock(string blockChecksum)
    {
        #region load block
        var blockEntity = await repoWrap.BlockRepo.QueryAll().FirstOrDefaultAsync(x => x.Checksum == blockChecksum);
        if (blockEntity == null)
        {
            throw new NotFoundUSException();
        }
        #endregion
        #region Load block from file
        string syspath = FileUtil.FileLibPathToSysPath(apps.GetAppsettings().Storage.LibraryLocation, blockEntity.FileLibraryPath);
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
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(createDto.LibraryId);
        var existingFile = await repoWrap.FileRepo.QueryAll()
            .FirstOrDefaultAsync(x => x.FileLibraryPath == createDto.FileLibraryPath);

        if (existingFile == null && createDto.Type == EFileTransactionType.PULL)
        {
            throw new NotFoundUSException();
        }

        if (existingFile == null && createDto.Type == EFileTransactionType.PUSH)
        {
            existingFile = await repoWrap.FileRepo.InsertAsync(new()
            {
                FileLibraryPath = createDto.FileLibraryPath,
            });
        }
        #endregion
        #region check if already locked

        var ongoingTransaction = existingFile.FileTransactions.FirstOrDefault(x => x.Status == EFileTransactionStatus.INCOMPLETE);
        if (ongoingTransaction != null)
        {
            throw new NotFoundUSException();
        }
        #endregion
        
        #region create transaction

        var newTransaction = new FileTransaction()
        {
            File = existingFile,
            Status = EFileTransactionStatus.INCOMPLETE,
            CreatedOn = DateTime.UtcNow,
            LastUpdatedOn = DateTime.UtcNow,
            Type = createDto.Type
        };
        existingFile.FileTransactions.Add(newTransaction);
        await repoWrap.FileTransactionRepo.InsertAsync(newTransaction);
        await repoWrap.FileRepo.UpdateAsync(existingFile);
        
        #endregion

        return new()
        {
            FileId = existingFile.Id,
            TransactionId = newTransaction.Id,
            Type = createDto.Type,
        };
    }
    #endregion
    #region POST - Finish transaction
    public async Task<FileTransactionFinishDto> FinishFileTransaction(Guid transactionId)
    {
        #region load transaction
        var transaction = await repoWrap.FileTransactionRepo.QueryAll().FirstOrDefaultAsync(x => x.Id == transactionId);
        if (transaction == null)
        {
            throw new NotFoundUSException();
        }
        
        if (transaction.Status == EFileTransactionStatus.COMPLETE)
        {
            throw new TransactionAlreadyCompleteUSException();
        }
        #endregion 
        
        #region checks
        if (transaction.ExpectedBlocks != transaction.ReceivedBlocks)
        {
            throw new TransactionBlockMismatchUSException();
        }
        
        transaction.Status = EFileTransactionStatus.COMPLETE;
        await repoWrap.FileTransactionRepo.UpdateAsync(transaction);
        #endregion
        
        throw new NotImplementedException();
    }
    #endregion
    #region POST - Push block
    public async Task<StatusDto> PushBlock(BlockPushDto blockDto)
    {
        #region load transaction
        var transaction = await repoWrap.FileTransactionRepo.QueryAll().FirstOrDefaultAsync(x => x.Id == blockDto.TransactionId);
        var lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(blockDto.LibraryId);
        if (transaction == null)
        {
            throw new NotFoundUSException();
        }
        #endregion

        await WriteTempBlock(blockDto.Checksum, blockDto.Content);
        
        await repoWrap.BlockRepo.InsertAsync(new()
        {
            Checksum = blockDto.Checksum,
            FileLibraryPath = blockDto.SourceFileLibraryPath,
            Library = lib,
            Offset = blockDto.Offset,
            Size = blockDto.Size
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
    public Task<LibraryDiffDto> CreateLibraryDiff(CreateDiffDto dto)
    {
        throw new NotImplementedException();
    }
    #endregion

    private async Task WriteTempBlock(string checksum, byte[] bytes)
    {
        string targetPath = Path.Join(apps.GetAppsettings().Storage.LibraryLocation, checksum);
        using (FileStream fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}