using System.Net.Mime;
using Gridify;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Main.Library;
using LamashareApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LamashareApi.Controllers;

public class LibraryController(ILibraryService ls) : BaseController
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create new library",
        Description = "Create a new library",
        OperationId = nameof(CreateLibrary)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(LibraryDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> CreateLibrary([FromBody] LibraryCreateDto cdto)
    {
        return Ok(await ls.CreateLibrary(cdto));
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