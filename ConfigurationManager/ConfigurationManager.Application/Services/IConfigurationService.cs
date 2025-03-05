using ConfigurationManager.ConfigurationManager.API.Models;

namespace ConfigurationManager.ConfigurationManager.Application.Services;

public interface IConfigurationService
{
    Task<List<ConfigurationDto>> GetAllConfigurationsAsync(Guid? userId = null, string? nameFilter = null,
        DateTime? createdAfter = null);

    Task<ConfigurationDto?> GetConfigurationByIdAsync(Guid id);
    Task<ConfigurationDto> CreateConfigurationAsync(ConfigurationCreateDto configurationCreateDto);
    Task<ConfigurationDto> UpdateConfigurationAsync(ConfigurationUpdateDto configurationUpdateDto);
    Task<ConfigurationVersionDto> GetConfigurationVersionAsync(Guid configurationVersionId);
    Task DeleteConfigurationAsync(Guid id);
}
