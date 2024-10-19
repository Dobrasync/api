using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Auth;

public class DeviceAS
{
    [Required]
    public string ClientId { get; set; } = default!;
}