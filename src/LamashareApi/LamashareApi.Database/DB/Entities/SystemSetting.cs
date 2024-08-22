namespace LamashareApi.Database.DB.Entities;

public class SystemSetting
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;
}