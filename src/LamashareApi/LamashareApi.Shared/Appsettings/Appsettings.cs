using System.ComponentModel.DataAnnotations;
using LamashareApi.Shared.Appsettings.System;

namespace LamashareApi.Shared.Appsettings;

public class Appsettings
{
    [Required]
    public CoreAS Core { get; set; }
}