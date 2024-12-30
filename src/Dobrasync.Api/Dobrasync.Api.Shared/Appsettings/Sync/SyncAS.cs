using System.ComponentModel.DataAnnotations;
using Dobrasync.Api.Shared.Appsettings.Sync.Transaction;

namespace Dobrasync.Api.Shared.Appsettings.Sync;

public class SyncAS
{
    /// <summary>
    ///     Settings related to file transactions.
    /// </summary>
    [Required]
    public TransactionAS Transaction { get; set; } = default!;
}