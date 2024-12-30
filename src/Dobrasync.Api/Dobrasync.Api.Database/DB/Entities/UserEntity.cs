using System.ComponentModel.DataAnnotations;
using Dobrasync.Api.Database.DB.Entities.Base;
using Dobrasync.Api.Shared.Permissions;

namespace Dobrasync.Api.Database.DB.Entities;

public class UserEntity : BaseEntity
{
    [MaxLength(64)] public string Username { get; set; } = default!;

    public EUserRole Role { get; set; } = default!;
    public virtual List<LibraryEntity> Libraries { get; set; } = new();
}