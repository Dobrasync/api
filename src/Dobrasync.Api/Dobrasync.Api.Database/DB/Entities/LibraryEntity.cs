using Dobrasync.Api.Database.DB.Entities.Base;

namespace Dobrasync.Api.Database.DB.Entities;

public class LibraryEntity : BaseEntity
{
    public string Name { get; set; } = default!;
}