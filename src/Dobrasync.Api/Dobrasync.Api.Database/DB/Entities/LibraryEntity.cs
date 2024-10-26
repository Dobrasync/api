using LamashareApi.Database.DB.Entities.Base;

namespace LamashareApi.Database.DB.Entities;

public class LibraryEntity : BaseEntity
{
    public string Name { get; set; } = default!;
}