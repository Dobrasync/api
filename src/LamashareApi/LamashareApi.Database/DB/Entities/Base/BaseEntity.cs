using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Database.DB.Entities.Base;

public abstract class BaseEntity
{
    [Key] public Guid Id { get; set; }
}