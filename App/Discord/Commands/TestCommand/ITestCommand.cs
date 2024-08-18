using Discord;

namespace App.Discord.Commands.TestCommand;

[SlashCommand("test", "Test command")]
[SlashCommandOption("name", "Name of the test", ApplicationCommandOptionType.String, true)]
[SlashCommandOption("value", "Value of the test", ApplicationCommandOptionType.String, false)]
public interface ITestCommand : IBaseSlashCommand;