namespace ConfigurationManager.ConfigurationManager.Domain.Entities;

public abstract class BaseConfigurationData;

public class ConfigurationDataColorSchemes : BaseConfigurationData
{
    public string Theme { get; set; } = "";
}

public class ConfigurationDataFonts : BaseConfigurationData
{
    public string FontFamily { get; set; } = "";
    public int FontSize { get; set; }
}
