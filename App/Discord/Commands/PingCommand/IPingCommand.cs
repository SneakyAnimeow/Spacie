using Discord;

namespace App.Discord.Commands.PingCommand;

[SlashCommand("ping", "Ping the bot")]
[SlashCommandOption("message", "The message to send", 
    ApplicationCommandOptionType.String, false)]
public interface IPingCommand : IBaseSlashCommand;