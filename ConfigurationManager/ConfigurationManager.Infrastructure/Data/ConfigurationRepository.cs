using ConfigurationManager.ConfigurationManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data;

public class ConfigurationRepository(ConfigurationDbContext context) : IConfigurationRepository
{
    public async Task<Configuration?> GetByIdAsync(Guid id)
    {
        return await context.Configurations.FindAsync(id);
    }

    public async Task<Configuration?> GetByIdAsync(Guid id, Guid userId)
    {
        return await context.Configurations.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task<List<Configuration>> GetAllAsync(Guid userId, string? nameFilter = null,
        DateTime? createdAfter = null)
    {
        IQueryable<Configuration> query = context.Configurations.Where(c => c.UserId == userId);

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(c => c.Name.Contains(nameFilter));
        }

        if (createdAfter.HasValue)
        {
            query = query.Where(c => c.DateAdded > createdAfter.Value);
        }

        return await query.ToListAsync();
    }

    public async Task AddAsync(Configuration configuration)
    {
        await context.Configurations.AddAsync(configuration);
    }

    public void Update(Configuration configuration)
    {
        context.Configurations.Update(configuration);
    }

    public void Remove(Configuration configuration)
    {
        context.Configurations.Remove(configuration);
    }

    public async Task<bool> ExistsAsync(Guid userId, string name)
    {
        return await context.Configurations.AnyAsync(c => c.UserId == userId && c.Name == name);
    }

    public async Task<BaseConfigurationVersion?> GetVersionByIdAsync(Guid versionId)
    {
        return await context.ConfigurationVersions.FindAsync(versionId);
    }

    public async Task<BaseConfigurationVersion?> GetVersionByIdAsync(Guid configurationId, Guid versionId, Guid userId)
    {
        var configuration =
            await context.Configurations.FirstOrDefaultAsync(c => c.Id == configurationId && c.UserId == userId);

        if (configuration == null)
        {
            return null;
        }

        return await context.ConfigurationVersions.FirstOrDefaultAsync(v =>
            v.Id == versionId && v.ConfigurationId == configurationId);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public IQueryable<Configuration> GetQueryable()
    {
        return context.Configurations.AsQueryable();
    }
}
