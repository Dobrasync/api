namespace LamashareApi.Database.DB.Entities;

public class Library : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Path { get; set; } = default!;
}