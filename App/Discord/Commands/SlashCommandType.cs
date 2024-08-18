namespace App.Discord.Commands;

/// <summary>
/// Represents the type of slash command.
/// </summary>
/// <remarks>
/// The slash command type determines where the command is valid:
/// - <see cref="DirectMessage"/>: Only valid in direct messages.
/// - <see cref="Guild"/>: Valid only within a specific guild.
/// - <see cref="Global"/>: Valid everywhere.
/// </remarks>
public enum SlashCommandType
{
    /// <summary>
    /// Represents the direct message slash command type.
    /// </summary>
    DirectMessage = 0,
    
    /// <summary>
    /// Represents the guild slash command type.
    /// </summary>
    Guild = 1,

    /// <summary>
    /// Represents the global slash command type.
    /// </summary>
    Global = 2
}