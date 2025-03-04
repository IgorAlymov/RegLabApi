using ConfigurationManager.ConfigurationManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data;

public class ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : DbContext(options)
{
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<ConfigurationVersion> ConfigurationVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Configuration>()
            .HasIndex(c => new { c.Name, c.UserId })
            .IsUnique();

        modelBuilder.Entity<Configuration>()
            .HasMany(c => c.ConfigurationVersions)
            .WithOne(cv => cv.Configuration)
            .HasForeignKey(cv => cv.ConfigurationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Configuration>()
            .HasOne(c => c.CurrentConfigurationVersion)
            .WithOne()
            .HasForeignKey<Configuration>(c => c.CurrentVersionId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deleting version that's in use

        modelBuilder.Entity<Configuration>()
            .Navigation(e => e.CurrentConfigurationVersion)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}
