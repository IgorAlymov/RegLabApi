using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigurationManager.ConfigurationManager.Domain.Entities;

public class ConfigurationVersion : BaseEntity
{
    public Guid ConfigurationId { get; set; }
    public Configuration? Configuration { get; set; }
    [Column(TypeName = "jsonb")] public string Data { get; set; }
}
