using Dobrasync.Api.BusinessLogic.Decorators;
using Dobrasync.Api.Database.Enums;

namespace Dobrasync.Api.BusinessLogic.Dtos.File;

public class FileTransactionCreateDto
{
    public EFileTransactionType Type { get; set; }
    public Guid LibraryId { get; set; }
    public string FileLibraryPath { get; set; } = default!;

    #region PUSH Fields

    /// <summary>
    ///     UTC time the file was last modified.
    ///     Required on PUSH
    ///     Ignored on PULL
    /// </summary>
    public DateTimeOffset DateModifiedFile { get; set; }

    /// <summary>
    ///     UTC time the file was created.
    ///     Required on PUSH
    ///     Ignored on PULL
    /// </summary>
    public DateTimeOffset DateCreatedFile { get; set; }

    /// <summary>
    ///     List of block checksums - in desired final order.
    ///     Required on PUSH
    ///     Ignored on PULL
    /// </summary>
    [RequiredIf(nameof(Type), EFileTransactionType.PUSH)]
    public string[]? BlockChecksums { get; set; }

    /// <summary>
    ///     The total checksum of the file after all blocks have been received
    ///     and re-assembled.
    ///     Required on PUSH
    ///     Ignored on PULL
    /// </summary>
    [RequiredIf(nameof(Type), EFileTransactionType.PUSH)]
    public string? TotalChecksum { get; set; }

    #endregion
}