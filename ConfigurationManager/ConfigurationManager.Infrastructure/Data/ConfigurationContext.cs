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

        base.OnModelCreating(modelBuilder);
    }
}
