using System.Text.Json;
using ConfigurationManager.ConfigurationManager.API.Models;
using ConfigurationManager.ConfigurationManager.Domain.Entities;

namespace ConfigurationManager.ConfigurationManager.Application;

public static class ConfigurationExtensions
{
    public static ConfigurationDto ToDto(this Configuration configuration) =>
        new()
        {
            Id = configuration.Id,
            Name = configuration.Name,
            Description = configuration.Description,
            CurrentVersionId = configuration.CurrentVersionId,
            CurrentConfigurationVersionDto = configuration.CurrentConfigurationVersion?.ToDto()
        };

    public static ConfigurationVersionDto ToDto(this ConfigurationVersion configurationVersion) =>
        new() { Id = configurationVersion.Id, Data = JsonSerializer.Deserialize<object>(configurationVersion.Data) };
}
