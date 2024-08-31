namespace App.Discord.Commands.GameCommand;

[SlashCommand("game", "Start a new game", SlashCommandType.Guild)]
public interface IGameCommand : IBaseSlashCommand;