using ConfigurationManager.ConfigurationManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data.Repositories;

public class ConfigurationVersionRepository(ConfigurationDbContext context) : IRepository<ConfigurationVersion>
{
    public async Task<ConfigurationVersion?> GetByIdAsync(Guid id, CancellationToken token = default) =>
        await context.ConfigurationVersions.FindAsync([id], cancellationToken: token);

    public async Task<IEnumerable<ConfigurationVersion>> GetAllAsync(CancellationToken token = default) =>
        await context.ConfigurationVersions.ToListAsync(cancellationToken: token);

    public async Task<ConfigurationVersion> AddAsync(ConfigurationVersion entity, CancellationToken token = default)
    {
        context.ConfigurationVersions.Add(entity);
        await context.SaveChangesAsync(token);
        return entity;
    }

    public async Task UpdateAsync(ConfigurationVersion entity, CancellationToken token = default)
    {
        context.ConfigurationVersions.Update(entity);
        await context.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var station = await context.ConfigurationVersions.FindAsync([id], cancellationToken: token);
        if (station != null)
        {
            context.ConfigurationVersions.Remove(station);
            await context.SaveChangesAsync(token);
        }
    }
}
