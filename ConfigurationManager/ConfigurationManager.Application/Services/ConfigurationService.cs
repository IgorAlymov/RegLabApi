using System.Text.Json;
using ConfigurationManager.ConfigurationManager.API.Models;
using ConfigurationManager.ConfigurationManager.Domain.Entities;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data.Repositories;

namespace ConfigurationManager.ConfigurationManager.Application.Services;

public class ConfigurationService(
    IConfigurationRepository<Configuration> configurationRepository,
    IRepository<ConfigurationVersion> configurationVersionRepository)
    : IConfigurationService
{
    public async Task<List<ConfigurationDto>> GetAllConfigurationsAsync(Guid? userId = null, string? nameFilter = null,
        DateTime? createdAfter = null)
    {
        var configurations = await configurationRepository.GetAllAsync(userId, nameFilter, createdAfter);
        return configurations.Select(configuration => configuration.ToDto()).ToList();
    }

    public async Task<ConfigurationDto?> GetConfigurationByIdAsync(Guid id)
    {
        var configuration = await configurationRepository.GetByIdAsync(id);
        return configuration?.ToDto();
    }

    public async Task<ConfigurationDto> CreateConfigurationAsync(ConfigurationCreateDto configurationCreateDto)
    {
        if (await configurationRepository.ExistsAsync(configurationCreateDto.UserId, configurationCreateDto.Name))
        {
            throw new InvalidOperationException("Configuration with this name already exists for this user.");
        }

        var configuration = new Configuration
        {
            UserId = configurationCreateDto.UserId,
            Name = configurationCreateDto.Name,
            Description = configurationCreateDto.Description
        };

        var initialVersion = new ConfigurationVersion
        {
            ConfigurationId = configuration.Id,
            Data = JsonSerializer.Serialize(configurationCreateDto.Data)
        };
        configuration.CurrentVersionId = initialVersion.Id;

        await configurationRepository.AddAsync(configuration);
        await configurationVersionRepository.AddAsync(initialVersion);
        return configuration.ToDto();
    }

    public async Task<ConfigurationDto> UpdateConfigurationAsync(Guid configurationId, ConfigurationUpdateDto configurationUpdateDto)
    {
        var configuration = await configurationRepository.GetByIdAsync(configurationId);
        if (configuration == null)
        {
            throw new KeyNotFoundException("Configuration not found.");
        }

        configuration.Name = configurationUpdateDto.Name;
        configuration.Description = configurationUpdateDto.Description;
        configuration.DateUpdated = DateTimeOffset.UtcNow;

        if (configurationUpdateDto.ConfigurationVersionId != null)
        {
            configuration.CurrentVersionId = configurationUpdateDto.ConfigurationVersionId.Value;
        }

        if (configurationUpdateDto.Data != null)
        {
            var newVersion = new ConfigurationVersion
            {
                ConfigurationId = configuration.Id, Data = JsonSerializer.Serialize(configurationUpdateDto.Data)
            };

            await configurationVersionRepository.AddAsync(newVersion);
            configuration.CurrentVersionId = newVersion.Id;
        }

        await configurationRepository.UpdateAsync(configuration);
        return configuration.ToDto();
    }

    public async Task<ConfigurationVersionDto?> GetConfigurationVersionAsync(Guid configurationId,
        Guid configurationVersionId)
    {
        var configuration = await configurationRepository.GetByIdAsync(configurationId);
        if (configuration == null)
        {
            throw new KeyNotFoundException("Configuration not found.");
        }

        var version = await configurationVersionRepository.GetByIdAsync(configurationVersionId);
        return version?.ToDto();
    }

    public async Task DeleteConfigurationAsync(Guid id)
    {
        var configuration = await configurationRepository.GetByIdAsync(id);
        if (configuration == null)
        {
            throw new KeyNotFoundException("Configuration not found.");
        }

        await configurationRepository.DeleteAsync(id);
    }
}
