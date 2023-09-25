using Microsoft.EntityFrameworkCore;
using OMGivens.ThenIncludeWhereBug.MinimalRepro.Entities;

namespace OMGivens.ThenIncludeWhereBug.MinimalRepro.Database;

public class ReproDbContext : DbContext
{
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Rolodex> Rolodexes => Set<Rolodex>();
    public DbSet<Client> Clients => Set<Client>();

    public ReproDbContext(DbContextOptions<ReproDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Business>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Business>()
            .HasOne(b => b.Rolodex)
            .WithOne(r => r.Business);

        modelBuilder.Entity<Rolodex>()
            .HasKey(b => b.BusinessId);

        modelBuilder.Entity<Client>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Rolodex>()
            .HasMany(r => r.Clients)
            .WithOne()
            .HasForeignKey(c => c.RolodexId);
    }
}