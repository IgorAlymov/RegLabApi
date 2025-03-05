namespace ConfigurationManager.ConfigurationManager.API.Models;

public class ConfigurationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid CurrentVersionId { get; set; }
    public ConfigurationVersionDto? CurrentConfigurationVersionDto { get; set; }
}
