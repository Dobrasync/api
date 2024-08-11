using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Database.DB.Entities;

public class File : BaseEntity
{
    public Guid Id { get; set; }

    [MaxLength(4096)] public string AbsolutePath { get; set; } = default!;
}