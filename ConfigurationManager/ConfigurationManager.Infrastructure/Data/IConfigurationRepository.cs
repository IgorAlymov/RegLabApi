using ConfigurationManager.ConfigurationManager.Domain.Entities;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data;

public interface IConfigurationRepository
{
    Task<Configuration?> GetByIdAsync(Guid id);
    Task<Configuration?> GetByIdAsync(Guid id, Guid userId);
    Task<List<Configuration>> GetAllAsync(Guid userId, string? nameFilter = null, DateTime? createdAfter = null);
    Task AddAsync(Configuration configuration);
    void Update(Configuration configuration);
    void Remove(Configuration configuration);
    Task<bool> ExistsAsync(Guid userId, string name);

    Task<BaseConfigurationVersion?> GetVersionByIdAsync(Guid versionId);
    Task<BaseConfigurationVersion?> GetVersionByIdAsync(Guid configurationId, Guid versionId, Guid userId);

    Task SaveChangesAsync();

    IQueryable<Configuration> GetQueryable();
}
