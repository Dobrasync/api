using Dobrasync.Api.BusinessLogic.Dtos.File;
using Dobrasync.Api.BusinessLogic.Dtos.Generic;

namespace Dobrasync.Api.BusinessLogic.Services.Main.File;

public interface IFileService
{
    #region GET - File total checksum

    public Task<FileChecksumDto> GetTotalChecksum(Guid libraryId, string libraryFilePath);

    #endregion

    #region GET - File info

    public Task<FileInfoDto> GetFileInfo(Guid libraryId, string libraryFilePath);

    #endregion

    #region GET - File status

    public Task<FileStatusDto> GetFileStatus(Guid libraryId, string libraryFilePath);

    #endregion

    #region GET - Get block list

    public Task<string[]> GetFileBlockList(Guid libraryId, string libraryFilePath);

    #endregion

    #region GET - Get block

    public Task<BlockDto> PullBlock(string blockChecksum);

    #endregion

    #region POST - Create file sync transaction

    public Task<FileTransactionDto> CreateFileTransaction(FileTransactionCreateDto createDto);

    #endregion

    #region POST - Finish file sync transaction

    public Task<FileTransactionFinishDto> FinishFileTransaction(Guid transactionId);

    #endregion

    #region POST - Push a block

    public Task<StatusDto> PushBlock(BlockPushDto blockDto);

    #endregion

    #region POST - Create diff

    public Task<LibraryDiffDto> CreateLibraryDiff(CreateDiffDto dto);

    #endregion

    #region DELETE - Delete file

    public Task DeleteFile(Guid libraryId, string libraryFilePath);

    #endregion
}