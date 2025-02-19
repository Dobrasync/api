using Dobrasync.Api.ApiControllers.Controllers.Base;
using Dobrasync.Api.BusinessLogic.Dtos.File;
using Dobrasync.Api.BusinessLogic.Dtos.Generic;
using Dobrasync.Api.BusinessLogic.Services.Main.File;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dobrasync.Api.ApiControllers.Controllers;

public class FileController(IFileService fileService) : BaseController
{
    #region GET - File total checksum

    [HttpGet("total-checksum")]
    [SwaggerOperation(
        Summary = "Get file total checksum",
        Description = "Retrieves a files total checksum. This checksum includes metadata.",
        OperationId = nameof(GetTotalChecksum)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(FileChecksumDto))]
    public async Task<IActionResult> GetTotalChecksum([FromQuery] Guid libraryId, string libraryFilePath)
    {
        return Ok(await fileService.GetTotalChecksum(libraryId, libraryFilePath));
    }

    #endregion

    #region GET - File info

    [HttpGet("info")]
    [SwaggerOperation(
        Summary = "Get file info",
        Description = "Retrieves a files info (last modified, etc). This includes status info.",
        OperationId = nameof(GetFileInfo)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(FileInfoDto))]
    public async Task<IActionResult> GetFileInfo([FromQuery] Guid libraryId, string libraryFilePath)
    {
        return Ok(await fileService.GetFileInfo(libraryId, libraryFilePath));
    }

    #endregion

    #region GET - File status

    [HttpGet("status")]
    [SwaggerOperation(
        Summary = "Get file status",
        Description =
            "Retrieves a files status (e.g. if it is locked by another sync process). This is  more lean than getting the full file info which might not even be needed most of the time.",
        OperationId = nameof(GetFileStatus)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(FileStatusDto))]
    public async Task<IActionResult> GetFileStatus([FromQuery] Guid libraryId, string libraryFilePath)
    {
        return Ok(await fileService.GetFileStatus(libraryId, libraryFilePath));
    }

    #endregion

    #region GET - Get block list

    [HttpGet("blocks")]
    [SwaggerOperation(
        Summary = "Get file block list",
        Description = "Gets a files blocks checksums.",
        OperationId = nameof(GetFileBlockList)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(string[]))]
    public async Task<IActionResult> GetFileBlockList([FromQuery] Guid libraryId, string libraryFilePath)
    {
        return Ok(await fileService.GetFileBlockList(libraryId, libraryFilePath));
    }

    #endregion

    #region GET - Get block

    [HttpGet("block")]
    [SwaggerOperation(
        Summary = "Pull a block",
        Description = "Gets a files block by its checksum.",
        OperationId = nameof(PullBlock)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(BlockDto))]
    public async Task<IActionResult> PullBlock([FromQuery] string blockChecksum)
    {
        return Ok(await fileService.PullBlock(blockChecksum));
    }

    #endregion

    #region POST - Create file sync transaction

    [HttpPost("transactions")]
    [SwaggerOperation(
        Summary = "Create file transaction",
        Description =
            "Creates a new file transaction. This locks the file and prevents other from writing to it as long as the transaction is running.",
        OperationId = nameof(CreateFileTransaction)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(FileTransactionDto))]
    public async Task<IActionResult> CreateFileTransaction([FromBody] FileTransactionCreateDto createDto)
    {
        return Ok(await fileService.CreateFileTransaction(createDto));
    }

    #endregion

    #region POST - Finish file sync transaction

    [HttpPost("transactions/finish")]
    [SwaggerOperation(
        Summary = "Finish file transaction",
        Description =
            "Finishes a file transaction. This tells the API to run checks and unlock the file if everything went well.",
        OperationId = nameof(FinishFileTransaction)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(FileTransactionFinishDto))]
    public async Task<IActionResult> FinishFileTransaction([FromQuery] Guid transactionId)
    {
        return Ok(await fileService.FinishFileTransaction(transactionId));
    }

    #endregion

    #region POST - Push a block

    [HttpPost("block")]
    [SwaggerOperation(
        Summary = "Push a block",
        Description = "Pushes a block.",
        OperationId = nameof(PushBlock)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(StatusDto))]
    public async Task<IActionResult> PushBlock([FromBody] BlockPushDto blockDto)
    {
        return Ok(await fileService.PushBlock(blockDto));
    }

    #endregion

    #region POST - Create diff list

    [HttpPost("diff")]
    [SwaggerOperation(
        Summary = "Get library diff",
        Description = "Generates a diff manifest.",
        OperationId = nameof(GetDiff)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(LibraryDiffDto))]
    public async Task<IActionResult> GetDiff([FromBody] CreateDiffDto dto)
    {
        return Ok(await fileService.CreateLibraryDiff(dto));
    }

    #endregion

    #region DELETE - Delete a file

    [HttpDelete("{libraryId}")]
    [SwaggerOperation(
        Summary = "Delete a file",
        Description = "Deletes the given file and its blocks.",
        OperationId = nameof(DeleteFile)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok))]
    public async Task<IActionResult> DeleteFile(Guid libraryId, [FromQuery] string fileLibraryPath)
    {
        await fileService.DeleteFile(libraryId, fileLibraryPath);
        return Ok();
    }

    #endregion
}