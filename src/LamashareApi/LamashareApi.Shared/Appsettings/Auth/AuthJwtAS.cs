using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Auth;

public class AuthJwtAS
{
    [Required] public string Secret { get; set; } = default!;
    [Required] public string Issuer { get; set; } = default!;
    [Required] public string Audience { get; set; } = default!;
    [Required] public int LifetimeMinutes { get; set; } = default!;
}