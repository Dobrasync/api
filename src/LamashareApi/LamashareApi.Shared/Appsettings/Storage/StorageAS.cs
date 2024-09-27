using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Storage;

public class StorageAS
{
    /// <summary>
    /// Where on the system all the libraries are supposed
    /// to be stored.
    /// </summary>
    [Required]
    public string LibraryLocation { get; set; } = default!;
}