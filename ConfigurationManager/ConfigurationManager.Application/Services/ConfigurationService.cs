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

        // Create Initial Version
        var initialVersion = new ConfigurationVersion
        {
            ConfigurationId = configuration.Id,
            Data = JsonSerializer.Serialize(createDto.Data), // Serialize to JSON
        };

        configuration.ConfigurationVersions.Add(initialVersion);
        configuration.CurrentVersionId = initialVersion.Id; //Set current Version
        configuration.CurrentConfigurationVersion = initialVersion; // Set current Version relation

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

        // Create New Version
        var newVersion = new ConfigurationVersion
        {
            ConfigurationId = configuration.Id,
            Data = JsonSerializer.Serialize(updateDto.Data), // Serialize to JSON
        };

        configuration.ConfigurationVersions.Add(newVersion);
        configuration.DateUpdated = DateTime.UtcNow;
        configuration.CurrentVersionId = newVersion.Id;

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
            Data = configuration.CurrentConfigurationVersion?.Data != null
                ? JsonSerializer.Deserialize<object>(configuration.CurrentConfigurationVersion.Data)
                : null, // Deserialize the JSON
            CurrentVersionId = configuration.CurrentVersionId
        };
    }
}
