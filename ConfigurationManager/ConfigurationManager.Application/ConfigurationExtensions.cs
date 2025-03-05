using ConfigurationManager.ConfigurationManager.API.Models;
using ConfigurationManager.ConfigurationManager.Domain.Entities;
using Newtonsoft.Json;

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
            CurrentConfigurationVersionDto = configuration.ConfigurationVersions
                .First(version => version.Id == configuration.CurrentVersionId).ToDto()
        };

    public static ConfigurationVersionDto ToDto(this BaseConfigurationVersion configurationVersion) =>
        new() { Id = configurationVersion.Id, Data = JsonConvert.SerializeObject(configurationVersion)};

    public static BaseConfigurationVersion? ToConfigurationVersionEntity(
        this IConfigurationOperation configurationCreateDto, Guid configurationId)
    {
        BaseConfigurationVersion? baseConfigurationVersion = null;
        switch (configurationCreateDto.ConfigurationType)
        {
            case ConfigurationType.ColorSchemes:
                baseConfigurationVersion = new ColorSchemesConfigurationVersion
                {
                    ConfigurationId = configurationId,
                    ConfigurationData =
                        JsonConvert.DeserializeObject<ConfigurationDataColorSchemes>(configurationCreateDto.Data)
                };
                break;
            case ConfigurationType.Fonts:
                baseConfigurationVersion = new FontsConfigurationVersion
                {
                    ConfigurationId = configurationId,
                    ConfigurationData =
                        JsonConvert.DeserializeObject<ConfigurationDataFonts>(configurationCreateDto.Data)
                };
                break;
        }

        return baseConfigurationVersion;
    }
}
