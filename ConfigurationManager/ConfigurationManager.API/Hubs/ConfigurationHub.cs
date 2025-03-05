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
}
