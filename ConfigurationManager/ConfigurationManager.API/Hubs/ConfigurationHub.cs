using ConfigurationManager.ConfigurationManager.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace ConfigurationManager.ConfigurationManager.API.Hubs;

public class ConfigurationHub(ILogger<ConfigurationHub> logger) : Hub
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

    public async Task SendConfigurationCreatedAsync(ConfigurationDto configuration)
    {
        logger.LogInformation($"Sending ConfigurationCreated notification for ConfigurationId: {configuration.Id}");
        await Clients.All.SendAsync("ReceiveConfigurationCreated", configuration);
    }

    public async Task SendConfigurationUpdatedAsync(ConfigurationDto configuration)
    {
        logger.LogInformation($"Sending ConfigurationUpdated notification for ConfigurationId: {configuration.Id}");
        await Clients.All.SendAsync("ReceiveConfigurationUpdated", configuration);
    }

    public async Task SendConfigurationDeleted(Guid configurationId)
    {
        logger.LogInformation($"Sending ConfigurationDeleted notification for ConfigurationId: {configurationId}");
        await Clients.All.SendAsync("ReceiveConfigurationDeleted", configurationId);
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
}
