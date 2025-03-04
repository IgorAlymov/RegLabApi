using ConfigurationManager.ConfigurationManager.Domain.Entities;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data;

public interface IConfigurationVersionRepository
{
    Task<ConfigurationVersion?> GetByIdAsync(Guid id);
    Task AddAsync(ConfigurationVersion configurationVersion);
    void Update(ConfigurationVersion configurationVersion);
    void Remove(ConfigurationVersion configurationVersion);
    Task SaveChangesAsync();
}
