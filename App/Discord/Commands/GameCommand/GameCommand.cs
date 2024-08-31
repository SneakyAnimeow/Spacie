using App.Services.GameService;
using Discord.WebSocket;

namespace App.Discord.Commands.GameCommand;

public class GameCommand : IGameCommand
{
    private readonly IGameService _gameService;
    
    public GameCommand(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    public async Task HandleAsync(SocketSlashCommand command)
    {
        await command.DeferAsync();
        
        // var image = await _gameService.GetOrCreateTTTAsync(command.GuildId, command.Channel.Id, command.User.Id);
    }
}