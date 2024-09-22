using System.Net.Mime;
using Lamashare.BusinessLogic.Dtos.File;
using Lamashare.BusinessLogic.Dtos.Generic;
using Lamashare.BusinessLogic.Dtos.Library;
using LamashareApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LamashareApi.Controllers;

public class FileController : BaseController
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
        return Ok();
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
        return Ok();
    }
    #endregion
    #region GET - File status
    [HttpGet("status")]
    [SwaggerOperation(
        Summary = "Get file status",
        Description = "Retrieves a files status (e.g. if it is locked by another sync process). This is  more lean than getting the full file info which might not even be needed most of the time.",
        OperationId = nameof(GetFileStatus)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(FileStatusDto))]
    public async Task<IActionResult> GetFileStatus([FromQuery] Guid libraryId, string libraryFilePath)
    {
        return Ok();
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
        return Ok();
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
        return Ok();
    }
    #endregion
    #region POST - Create file sync transaction
    [HttpPost("transactions")]
    [SwaggerOperation(
        Summary = "Create file transaction",
        Description = "Creates a new file transaction. This locks the file and prevents other from writing to it as long as the transaction is running.",
        OperationId = nameof(CreateFileTransaction)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(FileTransactionDto))]
    public async Task<IActionResult> CreateFileTransaction([FromBody] FileTransactionCreateDto createDto)
    {
        return Ok();
    }
    #endregion
    #region POST - Finish file sync transaction
    [HttpPost("transactions/finish")]
    [SwaggerOperation(
        Summary = "Finish file transaction",
        Description = "Finishes a file transaction. This tells the API to run checks and unlock the file if everything went well.",
        OperationId = nameof(FinishFileTransaction)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(FileTransactionFinishDto))]
    public async Task<IActionResult> FinishFileTransaction([FromQuery] Guid transactionId)
    {
        return Ok();
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
    public async Task<IActionResult> PushBlock([FromBody] BlockDto blockDto)
    {
        return Ok();
    }
    #endregion
}