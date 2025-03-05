namespace ConfigurationManager.ConfigurationManager.API.Models;

public class ConfigurationUpdateDto
{
    public object Data { get; set; }
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
}
