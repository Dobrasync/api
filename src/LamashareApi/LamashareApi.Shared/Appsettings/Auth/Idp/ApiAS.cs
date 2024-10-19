using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Auth;

public class ApiAS
{
    [Required] public string ClientId { get; set; } = default!;

    [Required] public string ClientSecret { get; set; } = default!;
}