using ConfigurationManager.ConfigurationManager.API.Models;
using ConfigurationManager.ConfigurationManager.Domain.Entities;
using ConfigurationManager.ConfigurationManager.Domain.Events;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data.Repositories;
using MediatR;
using Sitko.Core.App.Results;

namespace ConfigurationManager.ConfigurationManager.Application.Services;

public class ConfigurationService(
    IConfigurationRepository<Configuration> configurationRepository,
    IRepository<BaseConfigurationVersion> configurationVersionRepository,
    IPublisher mediator,
    ILogger<ConfigurationService> logger)
    : IConfigurationService
{
    private const string ConfigurationExistError = "\"Configuration with this name already exists for this user.\"";

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

    public async Task<OperationResult<ConfigurationDto>> CreateConfigurationAsync(
        ConfigurationCreateDto configurationCreateDto)
    {
        if (await configurationRepository.ExistsAsync(configurationCreateDto.UserId, configurationCreateDto.Name))
        {
            logger.LogError(ConfigurationExistError);
            return new OperationResult<ConfigurationDto>(ConfigurationExistError);
        }

        var configuration = new Configuration
        {
            UserId = configurationCreateDto.UserId,
            Name = configurationCreateDto.Name,
            Description = configurationCreateDto.Description
        };

        var initialVersion = configurationCreateDto.ToConfigurationVersionEntity(configuration.Id);
        configuration.CurrentVersionId = initialVersion.Id;

        await configurationRepository.AddAsync(configuration);
        await configurationVersionRepository.AddAsync(initialVersion);

        await mediator.Publish(new ConfigurationCreatedEvent(configuration.ToDto()));
        return new OperationResult<ConfigurationDto>(configuration.ToDto());
    }

    public async Task<ConfigurationDto> UpdateConfigurationAsync(Guid configurationId,
        ConfigurationUpdateDto configurationUpdateDto)
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
            var newVersion = configurationUpdateDto.ToConfigurationVersionEntity(configuration.Id);
            await configurationVersionRepository.AddAsync(newVersion);
            configuration.CurrentVersionId = newVersion.Id;
        }

        await configurationRepository.UpdateAsync(configuration);
        await mediator.Publish(new ConfigurationUpdatedEvent(configuration.ToDto()));
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
        await mediator.Publish(new ConfigurationDeletedEvent(id));
    }
}
