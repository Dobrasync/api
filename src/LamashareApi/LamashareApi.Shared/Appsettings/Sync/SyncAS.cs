using System.ComponentModel.DataAnnotations;
using LamashareApi.Shared.Appsettings.Sync.Transactions;

namespace LamashareApi.Shared.Appsettings.Sync;

public class SyncAS
{
    /// <summary>
    /// Settings related to file transactions.
    /// </summary>
    [Required]
    public TransactionAS Transaction { get; set; } = default!;
}