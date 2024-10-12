using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Shared.Appsettings.Sync.Transactions;

public class TransactionAS
{
    /// <summary>
    /// Maximum time a transaction can not be updated before
    /// it is automatically marked as "aborted" by the api
    /// and locks are removed.
    /// </summary>
    [Required]
    public int MaxStaleTimeSeconds { get; set; } = 60 * 5;
}