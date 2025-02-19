using System.Net.Mime;
using Dobrasync.Api.ApiControllers.Controllers.Base;
using Dobrasync.Api.BusinessLogic.Dtos.Library;
using Dobrasync.Api.BusinessLogic.Services.Main.Library;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dobrasync.Api.ApiControllers.Controllers;

public class LibraryController(ILibraryService ls) : BaseController
{
    [HttpPost]
    [SwaggerOperation(
        OperationId = nameof(CreateLibrary)
    )]
    public async Task<ActionResult<LibraryDto>> CreateLibrary([FromBody] LibraryCreateDto cdto)
    {
        LibraryDto dto = await ls.CreateLibrary(cdto);
        return Ok(dto);
    }

    [HttpGet("{libraryId}")]
    [SwaggerOperation(
        Summary = "Get a library with given id",
        Description = "Get a library with given id",
        OperationId = nameof(GetLibraryById)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(LibraryDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetLibraryById(Guid libraryId)
    {
        return Ok(await ls.GetLibraryById(libraryId));
    }

    [HttpPut("{libraryId}")]
    [SwaggerOperation(
        Summary = "Update a library of given id",
        Description = "Update a library of given id",
        OperationId = nameof(UpdateLibraryById)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(LibraryDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> UpdateLibraryById(Guid libraryId, [FromBody] LibraryUpdateDto udto)
    {
        return Ok(await ls.UpdateLibrary(libraryId, udto));
    }

    [HttpDelete("{libraryId}")]
    [SwaggerOperation(
        Summary = "Delete a library of given id",
        Description = "Delete a library of given id",
        OperationId = nameof(DeleteLibraryById)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(LibraryDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteLibraryById(Guid libraryId)
    {
        return Ok(await ls.DeleteLibrary(libraryId));
    }
}