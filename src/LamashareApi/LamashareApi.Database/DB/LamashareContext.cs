using LamashareApi.Database.DB.Entities;
using Microsoft.EntityFrameworkCore;
using File = LamashareApi.Database.DB.Entities.File;

namespace LamashareApi.Database.DB;

public class LamashareContext : DbContext
{
    public LamashareContext(DbContextOptions<LamashareContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
    }
    
    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Library> Libraries { get; set; }
}