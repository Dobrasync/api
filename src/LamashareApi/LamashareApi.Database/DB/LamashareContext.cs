using LamashareApi.Database.DB.Entities;
using Microsoft.EntityFrameworkCore;
using File = LamashareApi.Database.DB.Entities.File;

namespace LamashareApi.Database.DB;

public class LamashareContext : DbContext
{
    public LamashareContext(DbContextOptions<LamashareContext> options) : base(options)
    {
    }

    public virtual DbSet<SystemSetting> SystemSettings { get; set; }
    public virtual DbSet<FileTransaction> FileTransactions { get; set; }
    public virtual DbSet<Block> Blocks { get; set; }
    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Library> Libraries { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(e => e.Username)
            .IsUnique();
        
        builder.Entity<FileTransaction>()
            .Property(e => e.ExpectedBlocks)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        builder.Entity<FileTransaction>()
            .Property(e => e.ReceivedBlocks)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}