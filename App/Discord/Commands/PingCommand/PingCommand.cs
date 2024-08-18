using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace App.Discord.Commands.PingCommand;

public class PingCommand : IPingCommand
{
    private readonly ILogger<PingCommand> _logger;
    
    public PingCommand(ILogger<PingCommand> logger)
    {
        _logger = logger;
    }
    
    public async Task HandleAsync(SocketSlashCommand command)
    {
        _logger.LogInformation("User {user} pinged the bot", command.User);
        
        var message = command.Data.Options
            .FirstOrDefault(x => x.Name == "message")?.Value ?? "Pong!";
        await command.RespondAsync(message.ToString());
    }
}