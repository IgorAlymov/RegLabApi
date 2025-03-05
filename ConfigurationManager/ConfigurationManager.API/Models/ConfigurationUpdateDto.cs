namespace ConfigurationManager.ConfigurationManager.API.Models;

public class ConfigurationUpdateDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public object? Data { get; set; }
    public Guid? ConfigurationVersionId { get; set; }
}
