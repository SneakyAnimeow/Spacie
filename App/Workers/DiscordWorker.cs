using App.Services.DiscordClientService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Workers;

/// <summary>
/// Represents a worker that manages the Discord client service.
/// </summary>
public class DiscordWorker : IWorker, IAsyncDisposable
{
    private readonly ILogger<DiscordWorker> _logger;
    private readonly IDiscordClientService _discordClientService;
    private readonly IServiceScope _serviceScope;

    public DiscordWorker(ILogger<DiscordWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceScope = serviceProvider.CreateScope();
        _discordClientService = _serviceScope.ServiceProvider.GetRequiredService<IDiscordClientService>();
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Discord worker started");
        _discordClientService.StartAsync();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Discord worker stopped");
        _discordClientService.StopAsync();
        
        return Task.CompletedTask;
    }
    
    public ValueTask DisposeAsync()
    {
        _serviceScope.Dispose();
        GC.SuppressFinalize(this);
        
        return new ValueTask(Task.CompletedTask);
    }
}