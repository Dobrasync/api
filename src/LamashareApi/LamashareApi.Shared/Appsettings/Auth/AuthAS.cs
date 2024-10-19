using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Auth;

public class AuthAS
{
    [Required]
    public AuthJwtAS AuthJwt { get; set; } = default!;

    [Required] 
    public IdpAS Idp { get; set; } = default!;
}