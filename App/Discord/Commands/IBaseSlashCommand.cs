using Discord.WebSocket;

namespace App.Discord.Commands;

/// <summary>
/// Represents the base interface for slash commands.
/// </summary>
public interface IBaseSlashCommand
{
    /// <summary>
    /// Handles the asynchronous execution of a slash command.
    /// </summary>
    /// <param name="command">The <see cref="SocketSlashCommand"/> object representing the command.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task HandleAsync(SocketSlashCommand command);
}