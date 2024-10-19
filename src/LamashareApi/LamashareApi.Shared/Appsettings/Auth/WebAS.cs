using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Auth;

public class WebAS
{
    [Required]
    public string ClientId { get; set; } = default!;
}