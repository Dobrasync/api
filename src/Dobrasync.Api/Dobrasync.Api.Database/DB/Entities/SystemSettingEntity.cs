using Dobrasync.Api.Database.DB.Entities.Base;

namespace Dobrasync.Api.Database.DB.Entities;

public class SystemSettingEntity : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;
}