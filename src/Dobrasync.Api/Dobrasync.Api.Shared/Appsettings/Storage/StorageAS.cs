using System.ComponentModel.DataAnnotations;

namespace Dobrasync.Api.Shared.Appsettings.Storage;

public class StorageAS
{
    /// <summary>
    ///     Where on the system all the libraries are supposed
    ///     to be stored.
    /// </summary>
    [Required]
    public string LibraryLocation { get; set; } = default!;

    /// <summary>
    ///     Where blocks are temporarily stored
    /// </summary>
    [Required]
    public string TempBlockLocation { get; set; } = default!;
}