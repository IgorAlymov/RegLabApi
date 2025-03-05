using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigurationManager.ConfigurationManager.Domain.Entities;

public abstract class BaseConfigurationVersion : BaseEntity
{
    public Guid ConfigurationId { get; set; }
    public virtual ConfigurationType ConfigurationType { get; set; }
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
    public override ConfigurationType ConfigurationType { get; set; } = ConfigurationType.ColorSchemes;
}

public class FontsConfigurationVersion : BaseConfigurationVersion<ConfigurationDataFonts>
{
    public override ConfigurationType ConfigurationType { get; set; } = ConfigurationType.Fonts;
}
