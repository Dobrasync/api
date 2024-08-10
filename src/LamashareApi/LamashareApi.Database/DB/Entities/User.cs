namespace LamashareApi.Database.DB.Entities;

public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public virtual List<Library> Libraries { get; set; } = default!;
}