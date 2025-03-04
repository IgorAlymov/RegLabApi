using ConfigurationManager.ConfigurationManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data;

public class ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : DbContext(options)
{
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<BaseConfigurationVersion> ConfigurationVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Configuration>()
            .HasIndex(c => new { c.Name, c.UserId })
            .IsUnique();

        modelBuilder.Entity<BaseConfigurationVersion>()
            .HasDiscriminator<ConfigurationType>(nameof(BaseConfigurationVersion.ConfigurationType))
            .HasValue<ColorSchemesConfigurationVersion>(ConfigurationType.ColorSchemes)
            .HasValue<FontsConfigurationVersion>(ConfigurationType.Fonts);

        base.OnModelCreating(modelBuilder);
    }
}
