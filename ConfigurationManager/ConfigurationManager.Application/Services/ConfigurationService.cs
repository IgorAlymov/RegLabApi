using System.Text.Json;
using ConfigurationManager.ConfigurationManager.API.Models;
using ConfigurationManager.ConfigurationManager.Domain.Entities;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data;

namespace ConfigurationManager.ConfigurationManager.Application.Services;

public class ConfigurationService(
    IConfigurationRepository configurationRepository,
    IConfigurationVersionRepository configurationVersionRepository)
    : IConfigurationService
{
    public async Task<List<ConfigurationDto>> GetAllConfigurationsAsync(Guid userId, string? nameFilter = null,
        DateTime? createdAfter = null)
    {
        var configurations = await configurationRepository.GetAllAsync(userId, nameFilter, createdAfter);

        return configurations.Select(ToDto).ToList();
    }

    public async Task<ConfigurationDto?> GetConfigurationByIdAsync(Guid id, Guid userId)
    {
        var configuration = await configurationRepository.GetByIdAsync(id, userId);
        return configuration == null ? null : ToDto(configuration);
    }

    public async Task<ConfigurationDto> CreateConfigurationAsync(Guid userId, ConfigurationCreateDto createDto)
    {
        if (await configurationRepository.ExistsAsync(userId, createDto.Name))
        {
            throw new InvalidOperationException("Configuration with this name already exists for this user.");
        }

        var configuration = new Configuration
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = createDto.Name,
            Description = createDto.Description,
            DateAdded = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        // TODO: реализоват добавление версии

        await configurationRepository.AddAsync(configuration);
        await configurationRepository.SaveChangesAsync();

        return ToDto(configuration);
    }

    public async Task<ConfigurationDto> UpdateConfigurationAsync(Guid id, Guid userId, ConfigurationUpdateDto updateDto)
    {
        var configuration = await configurationRepository.GetByIdAsync(id, userId);

        if (configuration == null)
        {
            throw new KeyNotFoundException("Configuration not found.");
        }

        // TODO: реализоват добавление версии

        configuration.DateUpdated = DateTime.UtcNow;

        configurationRepository.Update(configuration);
        await configurationRepository.SaveChangesAsync();

        return ToDto(configuration);
    }

    public async Task<ConfigurationDto> GetConfigurationVersionAsync(Guid configurationId, Guid versionId, Guid userId)
    {
        var configuration = await configurationRepository.GetByIdAsync(configurationId, userId);

        if (configuration == null)
        {
            throw new KeyNotFoundException("Configuration not found.");
        }

        var version = await configurationRepository.GetVersionByIdAsync(configurationId, versionId, userId);
        if (version == null)
        {
            throw new KeyNotFoundException("Configuration version not found.");
        }

        return ToDto(configuration);
    }

    public async Task DeleteConfigurationAsync(Guid id, Guid userId)
    {
        var configuration = await configurationRepository.GetByIdAsync(id, userId);
        if (configuration == null)
        {
            throw new KeyNotFoundException("Configuration not found.");
        }

        configurationRepository.Remove(configuration);
        await configurationRepository.SaveChangesAsync();
    }

    private ConfigurationDto ToDto(Configuration configuration)
    {
        return new ConfigurationDto
        {
            Name = configuration.Name,
            Description = configuration.Description,
            CurrentVersionId = configuration.CurrentVersionId
        };
    }
}
