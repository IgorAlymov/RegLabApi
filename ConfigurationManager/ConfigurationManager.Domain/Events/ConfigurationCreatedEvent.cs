using ConfigurationManager.ConfigurationManager.API.Models;
using MediatR;

namespace ConfigurationManager.ConfigurationManager.Domain.Events;

public class ConfigurationCreatedEvent(ConfigurationDto configuration) : INotification
{
    public ConfigurationDto Configuration { get; } = configuration;
}

public class ConfigurationUpdatedEvent(ConfigurationDto configuration) : INotification
{
    public ConfigurationDto Configuration { get; } = configuration;
}

public class ConfigurationDeletedEvent(Guid configurationId) : INotification
{
    public Guid ConfigurationId { get; } = configurationId;
}
