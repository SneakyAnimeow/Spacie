using App.Services.AppInfoService;
using Discord;
using Discord.WebSocket;

namespace App.Discord.Commands.InfoCommand;

public class InfoCommand : IInfoCommand
{
    private readonly IAppInfoService _appInfoService;
    
    public InfoCommand(IAppInfoService appInfoService)
    {
        _appInfoService = appInfoService;
    }
    
    public async Task HandleAsync(SocketSlashCommand command)
    {
        var embed = new EmbedBuilder()
            .WithTitle("Bot Information")
            .WithDescription("Here is some information about the bot")
            .AddField("Name", await _appInfoService.GetNameAsync())
            .AddField("Version", await _appInfoService.GetVersionAsync())
            .AddField(".NET Version", await _appInfoService.GetDotnetVersionAsync())
            .AddField("Discord.NET Version", DiscordConfig.Version)
            .AddField("Discord API", DiscordConfig.APIVersion)
            .WithColor(Color.Blue)
            .WithCurrentTimestamp()
            .Build();
        
        await command.RespondAsync(embeds: [embed]);
    }
}