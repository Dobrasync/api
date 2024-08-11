using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.System;

public class CoreAS
{
    [Required]
    public string[] CorsOrigins { get; set; } = default!;
}