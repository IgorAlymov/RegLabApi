using ConfigurationManager.ConfigurationManager.Domain.Entities;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data;

public interface IConfigurationVersionRepository
{
    Task<BaseConfigurationVersion?> GetByIdAsync(Guid id);
    Task AddAsync(BaseConfigurationVersion configurationVersion);
    void Update(BaseConfigurationVersion configurationVersion);
    void Remove(BaseConfigurationVersion configurationVersion);
    Task SaveChangesAsync();
}
