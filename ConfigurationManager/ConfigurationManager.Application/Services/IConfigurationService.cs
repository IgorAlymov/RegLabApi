using ConfigurationManager.ConfigurationManager.API.Models;
using Sitko.Core.App.Results;

namespace ConfigurationManager.ConfigurationManager.Application.Services;

public interface IConfigurationService
{
    Task<List<ConfigurationDto>> GetAllConfigurationsAsync(Guid? userId = null, string? nameFilter = null,
        DateTime? createdAfter = null);

    Task<ConfigurationDto?> GetConfigurationByIdAsync(Guid id);
    Task<OperationResult<ConfigurationDto>> CreateConfigurationAsync(ConfigurationCreateDto configurationCreateDto);

    Task<ConfigurationDto>
        UpdateConfigurationAsync(Guid configurationId, ConfigurationUpdateDto configurationUpdateDto);

    Task<ConfigurationVersionDto?> GetConfigurationVersionAsync(Guid configurationId, Guid configurationVersionId);
    Task DeleteConfigurationAsync(Guid id);
}
