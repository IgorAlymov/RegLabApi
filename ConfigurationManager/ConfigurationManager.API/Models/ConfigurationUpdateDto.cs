using ConfigurationManager.ConfigurationManager.Domain.Entities;

namespace ConfigurationManager.ConfigurationManager.API.Models;

public class ConfigurationUpdateDto : IConfigurationOperation
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? Data { get; set; } = "";
    public Guid? ConfigurationVersionId { get; set; }
    public ConfigurationType ConfigurationType { get; set; }
}
