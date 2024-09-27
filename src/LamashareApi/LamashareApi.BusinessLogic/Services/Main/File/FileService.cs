using Lamashare.BusinessLogic.Dtos.File;
using Lamashare.BusinessLogic.Dtos.Generic;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Database.Repos;
using LamashareCore.Util;

namespace Lamashare.BusinessLogic.Services.Main.File;

public class FileService(IRepoWrapper repoWrap, IAppsettingsProvider apps) : IFileService
{
    public async Task<FileChecksumDto> GetTotalChecksum(Guid libraryId, string libraryFilePath)
    {
        #region Load library
        LamashareApi.Database.DB.Entities.Library lib = await repoWrap.LibraryRepo.GetByIdAsyncThrows(libraryId);
        #endregion
        string sysPath = FileUtil.FileLibPathToSysPath(apps.GetAppsettings().Storage.LibraryLocation, libraryFilePath);
        string check = await FileUtil.GetFileTotalChecksumAsync(sysPath);
        return new FileChecksumDto()
        {
            Checksum = check,
        };
    }

    public Task<FileInfoDto> GetFileInfo(Guid libraryId, string libraryFilePath)
    {
        throw new NotImplementedException();
    }

    public Task<FileStatusDto> GetFileStatus(Guid libraryId, string libraryFilePath)
    {
        throw new NotImplementedException();
    }

    public Task<string[]> GetFileBlockList(Guid libraryId, string libraryFilePath)
    {
        throw new NotImplementedException();
    }

    public Task<BlockDto> PullBlock(string blockChecksum)
    {
        throw new NotImplementedException();
    }

    public Task<FileTransactionDto> CreateFileTransaction(FileTransactionCreateDto createDto)
    {
        throw new NotImplementedException();
    }

    public Task<FileTransactionFinishDto> FinishFileTransaction(Guid transactionId)
    {
        throw new NotImplementedException();
    }

    public Task<StatusDto> PushBlock(BlockDto blockDto)
    {
        throw new NotImplementedException();
    }
}