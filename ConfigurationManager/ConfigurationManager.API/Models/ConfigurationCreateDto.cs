using ConfigurationManager.ConfigurationManager.Domain.Entities;

namespace ConfigurationManager.ConfigurationManager.API.Models;

public class ConfigurationCreateDto : IConfigurationOperation
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public ConfigurationType ConfigurationType { get; set; }
    public string Data { get; set; } = "";
    public Guid UserId { get; set; }
}

public interface IConfigurationOperation
{
    public string Data { get; set; }
    public ConfigurationType ConfigurationType { get; set; }
}
