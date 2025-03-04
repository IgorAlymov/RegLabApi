namespace ConfigurationManager.ConfigurationManager.API.Models;

public class ConfigurationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public object Data { get; set; } // JSON данные последней версии
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public Guid CurrentVersionId { get; set; }
}
