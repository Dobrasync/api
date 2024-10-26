using LamashareApi.Database.DB.Entities.Base;

namespace LamashareApi.Database.DB.Entities;

public class SystemSettingEntity : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;
}