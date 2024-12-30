using Dobrasync.Api.Database.DB.Entities;

namespace Dobrasync.Api.BusinessLogic.Services.Core.AccessControl;

public class AccessControl
{
    public UserEntity Invoker { get; set; } = default!;

    public void OwnsLibrary(LibraryEntity library)
    {
        if (!Invoker.Libraries.Contains(library)) throw new UnauthorizedAccessException();
    }
}