using LamashareApi.Database.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace LamashareApi.Database.DB;

public class LamashareContext : DbContext
{
    public LamashareContext(DbContextOptions<LamashareContext> options) : base(options)
    {
    }

    public virtual DbSet<SystemSettingEntity> SystemSettings { get; set; }
    public virtual DbSet<FileTransactionEntity> FileTransactions { get; set; }
    public virtual DbSet<BlockEntity> Blocks { get; set; }
    public virtual DbSet<FileEntity> Files { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<LibraryEntity> Libraries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserEntity>()
            .HasIndex(e => e.Username)
            .IsUnique();

        builder.Entity<FileEntity>()
            .HasIndex(e => e.FileLibraryPath)
            .IsUnique();

        builder.Entity<BlockEntity>()
            .HasIndex(e => e.Checksum)
            .IsUnique();

        builder.Entity<FileTransactionEntity>()
            .Property(e => e.TotalBlocks)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        builder.Entity<FileTransactionEntity>()
            .Property(e => e.ReceivedBlocks)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        builder.Entity<FileTransactionEntity>()
            .Property(e => e.RequiredBlocks)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}