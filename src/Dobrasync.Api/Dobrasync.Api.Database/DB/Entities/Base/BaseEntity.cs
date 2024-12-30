using System.ComponentModel.DataAnnotations;

namespace Dobrasync.Api.Database.DB.Entities.Base;

public abstract class BaseEntity
{
    [Key] public Guid Id { get; set; }
}