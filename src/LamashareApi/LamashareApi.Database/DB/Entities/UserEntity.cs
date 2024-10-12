using System.ComponentModel.DataAnnotations;
using LamashareApi.Shared.Permissions;

namespace LamashareApi.Database.DB.Entities;

public class UserEntity : BaseEntity
{
    [MaxLength(64)]
    public string Username { get; set; } = default!;

    [MaxLength(128)]
    public string Password { get; set; } = default!;
    
    public EUserRole Role { get; set; } = default!;
    
    public virtual List<LibraryEntity> Libraries { get; set; } = default!;
}