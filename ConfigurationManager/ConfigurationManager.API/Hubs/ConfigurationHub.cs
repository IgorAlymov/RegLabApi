using ConfigurationManager.ConfigurationManager.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ConfigurationManager.ConfigurationManager.API.Hubs;

public class ConfigurationHub(ILogger<ConfigurationHub> logger) : Hub,
    INotificationHandler<ConfigurationCreatedEvent>,
    INotificationHandler<ConfigurationUpdatedEvent>,
    INotificationHandler<ConfigurationDeletedEvent>
{
    public override async Task OnConnectedAsync()
    {
        logger.LogInformation($"Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        logger.LogInformation(
            $"Client disconnected: {Context.ConnectionId}, Reason: {exception?.Message ?? "No reason provided"}");
        await base.OnDisconnectedAsync(exception);
    }

    private Guid GetUserIdFromContext()
    {
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"];
        if (Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }

        throw new Exception("Unable to determine User ID");
    }

    public async Task Handle(ConfigurationCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sending ConfigurationCreated notification for ConfigurationId: {notification.Configuration.Id}");
        await Clients.All.SendAsync("ReceiveConfigurationCreated", notification.Configuration);
    }

    public async Task Handle(ConfigurationUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sending ConfigurationUpdated notification for ConfigurationId: {notification.Configuration.Id}");
        await Clients.All.SendAsync("ReceiveConfigurationUpdated", notification.Configuration);
    }

    public async Task Handle(ConfigurationDeletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Sending ConfigurationDeleted notification for ConfigurationId: {notification.ConfigurationId}");
        await Clients.All.SendAsync("ReceiveConfigurationDeleted", notification.ConfigurationId);
    }
}
