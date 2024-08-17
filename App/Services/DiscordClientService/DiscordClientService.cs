using App.Extensions;
using App.Utils;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Services.DiscordClientService;

/// <inheritdoc cref="IDiscordClientService"/>
public class DiscordClientService : IDiscordClientService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DiscordClientService> _logger;
    private readonly DiscordSocketClient _discord;
    
    public DiscordClientService(IConfiguration configuration, ILogger<DiscordClientService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _discord = new DiscordSocketClient();
    }

    public async Task<bool> StartAsync()
    {
        var token = _configuration.GetSection("DISCORD_TOKEN").Get<string?>();
        
        if (token is null)
        {
            _logger.LogError("Discord token not found in configuration");
            return false;
        }

        _discord.Log += (message) =>
        {
            StructUtils.ChangeReadonlyProperty(ref message, "Source", message.Source.ToAcronym());
            
            switch (message.Severity)
            {
                case LogSeverity.Critical or LogSeverity.Error:
                    _logger.LogError("({source}) {exception}\n{message}", message.Source, message.Message,
                        message.Exception);
                    break;
                case LogSeverity.Warning:
                    _logger.LogWarning("({source}) {message}", message.Source, message.Message);
                    break;
                case LogSeverity.Debug or LogSeverity.Verbose:
                    _logger.LogDebug("({source}) {message}", message.Source, message.Message);
                    break;
                default:
                    _logger.LogInformation("({source}) {message}", message.Source, message.Message);
                    break;
            }
            
            return Task.CompletedTask;
        };

        try
        {
            await _discord.LoginAsync(TokenType.Bot, token);
        }catch (Exception e)
        {
            _logger.LogError(e, "Failed to login to Discord");
            return false;
        }
        
        await _discord.StartAsync();
        
        return true;
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Stopping Discord client");
        return _discord.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _discord.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        _discord.Dispose();
        GC.SuppressFinalize(this);
    }
}