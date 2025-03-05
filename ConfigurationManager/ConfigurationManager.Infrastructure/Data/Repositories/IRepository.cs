using ConfigurationManager.ConfigurationManager.Domain.Entities;

namespace ConfigurationManager.ConfigurationManager.Infrastructure.Data.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<T> AddAsync(T entity, CancellationToken token = default);
    Task UpdateAsync(T entity, CancellationToken token = default);
    Task DeleteAsync(Guid id, CancellationToken token = default);
}

public interface IConfigurationRepository<T> : IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(Guid? userId = null, string? nameFilter = null, DateTime? createdAfter = null);
    Task<bool> ExistsAsync(Guid userId, string name);
}
