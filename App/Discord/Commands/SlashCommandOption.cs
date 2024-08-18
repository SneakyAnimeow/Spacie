using Discord;

namespace App.Discord.Commands;

/// <summary>
/// Attribute used to mark a class as a slash command option.
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
public class SlashCommandOption() : Attribute
{
    /// <summary>
    /// Represents slash command option name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Represents a slash command option description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Represents a slash command option type.
    /// </summary>
    public ApplicationCommandOptionType Type { get; set; } = ApplicationCommandOptionType.String;

    /// <summary>
    /// Represents a value indicating whether the option is required.
    /// </summary>
    public bool Required { get; set; } = false;
    
    public SlashCommandOption(string name) : this()
    {
        Name = name;
    }
    
    public SlashCommandOption(string name, string description) : this(name)
    {
        Description = description;
    }
    
    public SlashCommandOption(string name, string description, ApplicationCommandOptionType type) : this(name, description)
    {
        Type = type;
    }
    
    public SlashCommandOption(string name, string description, ApplicationCommandOptionType type, bool required) : this(name, description, type)
    {
        Required = required;
    }
}