using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Auth;

public class AuthAS
{
    [Required] public IdpAS Idp { get; set; } = default!;
}