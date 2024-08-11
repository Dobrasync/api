using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Auth;

public class AuthAS
{
    [Required] public string JwtSecret { get; set; } = default!;
}