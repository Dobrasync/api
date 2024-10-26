namespace LamashareApi.Shared.Localization;

public class LocKey
{
    public static string TemplatePrefix = "LOCKEY#";
    public string Key { get; set; } = default!;
    public string KeyTemplate => $"{TemplatePrefix}{Key}";
}