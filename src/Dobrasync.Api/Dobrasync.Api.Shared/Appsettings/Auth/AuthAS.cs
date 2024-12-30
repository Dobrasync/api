using System.ComponentModel.DataAnnotations;
using Dobrasync.Api.Shared.Appsettings.Auth.Idp;

namespace Dobrasync.Api.Shared.Appsettings.Auth;

public class AuthAS
{
    [Required] public IdpAS Idp { get; set; } = default!;
}