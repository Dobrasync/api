using System.ComponentModel.DataAnnotations;

namespace Dobrasync.Api.Shared.Appsettings.Auth.Idp;

public class ApiAS
{
    [Required] public string ClientId { get; set; } = default!;

    [Required] public string ClientSecret { get; set; } = default!;
}