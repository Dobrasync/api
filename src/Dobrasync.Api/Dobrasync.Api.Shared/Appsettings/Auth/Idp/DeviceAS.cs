using System.ComponentModel.DataAnnotations;

namespace Dobrasync.Api.Shared.Appsettings.Auth.Idp;

public class DeviceAS
{
    [Required] public string ClientId { get; set; } = default!;
}