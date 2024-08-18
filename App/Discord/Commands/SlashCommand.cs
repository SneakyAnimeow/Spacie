namespace App.Discord.Commands;

/// <summary>
/// Attribute used to mark a class as a slash command.
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public class SlashCommand() : Attribute
{
    /// <summary>
    /// Represents a slash command name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents a slash command description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents a slash command type.
    /// </summary>
    public SlashCommandType Type { get; set; } = SlashCommandType.Global;

    /// <summary>
    /// Determines the default permission for a slash command.
    /// </summary>
    public bool DefaultPermission { get; set; } = true;

    /// <summary>
    /// Represents the age restriction of a slash command.
    /// </summary>
    /// <value>
    /// <c>true</c> if the slash command has an age restriction; otherwise, <c>false</c>.
    /// </value>
    public bool AgeRestriction { get; set; } = false;
    
    public SlashCommand(string name) : this()
    {
        Name = name;
    }
    
    public SlashCommand(string name, string description) : this(name)
    {
        Description = description;
    }

    public SlashCommand(string name, string description, SlashCommandType type) : this(name, description)
    {
        Type = type;
    }

    public SlashCommand(string name, string description, SlashCommandType type, bool ageRestriction) : this(name, description, type)
    {
        AgeRestriction = ageRestriction;
    }

    public SlashCommand(string name, string description, SlashCommandType type, bool ageRestriction, bool defaultPermission) : this(name,
        description, type, ageRestriction)
    {
        DefaultPermission = defaultPermission;
    }
}