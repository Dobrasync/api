using System.ComponentModel.DataAnnotations;

namespace Dobrasync.Api.Shared.Appsettings.Core;

public class CoreAS
{
    [Required] public string[] CorsOrigins { get; set; } = default!;
}