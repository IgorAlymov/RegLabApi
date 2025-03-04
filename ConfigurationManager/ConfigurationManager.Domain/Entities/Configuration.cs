namespace ConfigurationManager.ConfigurationManager.Domain.Entities;

public class Configuration : BaseEntity
{
    public string Name { get; set; } = "";
    public Guid UserId { get; set; }
    public string Description { get; set; } = "";
    public Guid CurrentVersionId { get; set; }
    public ConfigurationVersion? CurrentConfigurationVersion { get; set; }
    public List<ConfigurationVersion> ConfigurationVersions { get; set; } = [];
}
