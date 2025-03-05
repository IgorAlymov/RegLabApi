using ConfigurationManager.ConfigurationManager.API.Hubs;
using ConfigurationManager.ConfigurationManager.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ConfigurationManager.ConfigurationManager.Application.EventHandlers;

public class ConfigurationCreatedEventHandler(
    IHubContext<ConfigurationHub> hubContext,
    ILogger<ConfigurationCreatedEventHandler> logger)
    : INotificationHandler<ConfigurationCreatedEvent>
{
    public async Task Handle(ConfigurationCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sending ConfigurationCreated notification for ConfigurationId: {notification.Configuration.Id}");
        await hubContext.Clients.All.SendAsync("ReceiveConfigurationCreated", notification.Configuration);
    }
}

public class ConfigurationUpdatedEventHandler(
    IHubContext<ConfigurationHub> hubContext,
    ILogger<ConfigurationUpdatedEventHandler> logger)
    : INotificationHandler<ConfigurationUpdatedEvent>
{
    public async Task Handle(ConfigurationUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sending ConfigurationUpdated notification for ConfigurationId: {notification.Configuration.Id}");
        await hubContext.Clients.All.SendAsync("ReceiveConfigurationUpdated", notification.Configuration);
    }
}

public class ConfigurationDeletedEventHandler(
    IHubContext<ConfigurationHub> hubContext,
    ILogger<ConfigurationDeletedEventHandler> logger)
    : INotificationHandler<ConfigurationDeletedEvent>
{
    public async Task Handle(ConfigurationDeletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sending ConfigurationDeleted notification for ConfigurationId: {notification.ConfigurationId}");
        await hubContext.Clients.All.SendAsync("ReceiveConfigurationDeleted", notification.ConfigurationId);
    }
}
