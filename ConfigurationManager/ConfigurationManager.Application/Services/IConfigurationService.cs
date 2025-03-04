using ConfigurationManager.ConfigurationManager.API.Models;

namespace ConfigurationManager.ConfigurationManager.Application.Services;

public interface IConfigurationService
{
    Task<List<ConfigurationDto>> GetAllConfigurationsAsync(Guid userId, string? nameFilter = null, DateTime? createdAfter = null);
    Task<ConfigurationDto?> GetConfigurationByIdAsync(Guid id, Guid userId);
    Task<ConfigurationDto> CreateConfigurationAsync(Guid userId, ConfigurationCreateDto createDto);
    Task<ConfigurationDto> UpdateConfigurationAsync(Guid id, Guid userId, ConfigurationUpdateDto updateDto);
    Task<ConfigurationDto> GetConfigurationVersionAsync(Guid configurationId, Guid versionId, Guid userId);
    Task DeleteConfigurationAsync(Guid id, Guid userId);
}
