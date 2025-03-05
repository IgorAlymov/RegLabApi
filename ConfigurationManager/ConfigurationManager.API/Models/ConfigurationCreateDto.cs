namespace ConfigurationManager.ConfigurationManager.API.Models;

public class ConfigurationCreateDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public object Data { get; set; }
    public Guid UserId { get; set; }
}
