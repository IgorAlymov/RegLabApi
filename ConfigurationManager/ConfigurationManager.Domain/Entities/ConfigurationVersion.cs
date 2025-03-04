using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigurationManager.ConfigurationManager.Domain.Entities;

public abstract class BaseConfigurationVersion : BaseEntity
{
    public Guid ConfigurationId { get; set; }
    public Configuration? Configuration { get; set; }
    public ConfigurationType ConfigurationType { get; set; }
}

public class BaseConfigurationVersion<TData> : BaseConfigurationVersion where TData : BaseConfigurationData
{
    [Column(TypeName = "jsonb")] public TData ConfigurationData { get; set; }
}

public enum ConfigurationType
{
    ColorSchemes = 0,
    Fonts = 1
}

public class ColorSchemesConfigurationVersion : BaseConfigurationVersion<ConfigurationDataColorSchemes>
{
}

public class FontsConfigurationVersion : BaseConfigurationVersion<ConfigurationDataFonts>
{
}
