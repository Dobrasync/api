using Lamashare.BusinessLogic.Services.Core.Invoker;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.DB.Entities.Base;
using Microsoft.Extensions.Options;

namespace Lamashare.BusinessLogic.Services.Core.AccessControl;

public class AccessControl
{
    public UserEntity Invoker { get; set; } = default!;

    public void OwnsLibrary(LibraryEntity library)
    {
        if (!Invoker.Libraries.Contains(library)) throw new UnauthorizedAccessException();
    }
}