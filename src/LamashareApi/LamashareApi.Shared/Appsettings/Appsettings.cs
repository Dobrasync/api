using System.ComponentModel.DataAnnotations;
using LamashareApi.Shared.Appsettings.Auth;
using LamashareApi.Shared.Appsettings.Storage;
using LamashareApi.Shared.Appsettings.Sync;
using LamashareApi.Shared.Appsettings.System;

namespace LamashareApi.Shared.Appsettings;

public class Appsettings
{
    /// <summary>
    /// Core configuration.
    /// </summary>
    [Required] 
    public CoreAS Core { get; set; } = default!;
    
    /// <summary>
    /// Authentication configuration
    /// </summary>
    [Required]
    public AuthAS Auth { get; set; } = default!;
    
    /// <summary>
    /// Storage configuration.
    /// </summary>
    [Required]
    public StorageAS Storage { get; set; } = default!;

    /// <summary>
    /// Settings related to sync logic.
    /// </summary>
    [Required] 
    public SyncAS Sync { get; set; } = default!;
}