using ConfigurationManager.ConfigurationManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data.Repositories;

public class ConfigurationRepository(ConfigurationDbContext context) : IConfigurationRepository<Configuration>
{
    public async Task<IEnumerable<Configuration>> GetAllAsync(CancellationToken token = default) =>
        await context.Configurations.ToListAsync(cancellationToken: token);

    public async Task<Configuration?> GetByIdAsync(Guid id, CancellationToken token = default) =>
        await context.Configurations.Include(configuration => configuration.ConfigurationVersions)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: token);

    public async Task<Configuration?> GetByIdAsync(Guid id, Guid userId) =>
        await context.Configurations.Include(configuration => configuration.ConfigurationVersions)
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

    public async Task<IEnumerable<Configuration>> GetAllAsync(Guid? userId = null, string? nameFilter = null,
        DateTime? createdAfter = null)
    {
        var query = context.Configurations.Include(configuration => configuration.ConfigurationVersions)
            .AsQueryable();
        if (userId != null)
        {
            query = context.Configurations.Where(c => c.UserId == userId);
        }

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(c => c.Name.Contains(nameFilter));
        }

        if (createdAfter.HasValue)
        {
            query = query.Where(c => c.DateAdded > createdAfter);
        }

        return await query.ToListAsync();
    }

    public async Task<Configuration> AddAsync(Configuration entity, CancellationToken token = default)
    {
        context.Configurations.Add(entity);
        await context.SaveChangesAsync(token);
        return entity;
    }

    public async Task UpdateAsync(Configuration entity, CancellationToken token = default)
    {
        context.Configurations.Update(entity);
        await context.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var station = await context.Configurations.FindAsync([id], cancellationToken: token);
        if (station != null)
        {
            context.Configurations.Remove(station);
            await context.SaveChangesAsync(token);
        }
    }

    public async Task<bool> ExistsAsync(Guid userId, string name) =>
        await context.Configurations.AnyAsync(c => c.UserId == userId && c.Name == name);
}
