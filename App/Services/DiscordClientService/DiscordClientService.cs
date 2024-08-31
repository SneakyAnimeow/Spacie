using System.Reflection;
using App.Discord.Commands;
using App.Extensions;
using App.Utils;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Services.DiscordClientService;

/// <inheritdoc cref="IDiscordClientService"/>
public class DiscordClientService : IDiscordClientService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DiscordClientService> _logger;
    private readonly DiscordSocketClient _discord;
    private readonly IServiceProvider _serviceProvider;

    public DiscordClientService(IConfiguration configuration, ILogger<DiscordClientService> logger,
        IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _logger = logger;
        _discord = new DiscordSocketClient();
        _serviceProvider = serviceProvider;
    }

    public async Task<bool> StartAsync()
    {
        var token = _configuration.GetSection("DISCORD_TOKEN").Get<string?>();

        if (token is null)
        {
            _logger.LogError("Discord token not found in configuration");
            return false;
        }

        _discord.Log += (message) =>
        {
            StructUtils.ChangeReadonlyProperty(ref message, "Source", message.Source.ToAcronym());

            switch (message.Severity)
            {
                case LogSeverity.Critical or LogSeverity.Error:
                    _logger.LogError("({source}) {exception}\n{message}", message.Source, message.Message,
                        message.Exception);
                    break;
                case LogSeverity.Warning:
                    _logger.LogWarning("({source}) {message}", message.Source, message.Message);
                    break;
                case LogSeverity.Debug or LogSeverity.Verbose:
                    _logger.LogDebug("({source}) {message}", message.Source, message.Message);
                    break;
                default:
                    _logger.LogInformation("({source}) {message}", message.Source, message.Message);
                    break;
            }

            return Task.CompletedTask;
        };

        _discord.Ready += RegisterCommandsAsync;
        _discord.SlashCommandExecuted += SlashCommandHandlerLogger;

        try
        {
            await _discord.LoginAsync(TokenType.Bot, token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to login to Discord");
            return false;
        }

        await _discord.StartAsync();

        return true;
    }

    /// <summary>
    /// Handles logging of slash command execution.
    /// </summary>
    /// <param name="context">The socket slash command context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private Task SlashCommandHandlerLogger(SocketSlashCommand context)
    {
        _logger.LogDebug("Handling slash command {command} from guild {guild} sent by {user}", context.CommandName,
            context.GuildId, context.User.Id);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Registers all slash commands in the application.
    /// </summary>
    /// <remarks>
    /// This method unregisters all existing global and guild-level slash commands and registers new
    /// ones based on the interfaces decorated with the <see cref="SlashCommand"/> attribute.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <seealso cref="SlashCommand"/>
    /// <seealso cref="SlashCommandOption"/>
    private async Task RegisterCommandsAsync()
    {
        //unregister all commands
        await _discord.Rest.DeleteAllGlobalCommandsAsync();

        foreach (var task in _discord.Guilds.Select(guild => guild.DeleteApplicationCommandsAsync()))
            await task;

        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();

        // Find all interfaces that are decorated with the SlashCommand attribute
        var commandInterfaces = types.Where(x => x.GetCustomAttribute<SlashCommand>() is not null && x.IsInterface);

        foreach (var commandInterface in commandInterfaces)
        {
            // Find the implementation that implements this interface
            var implementation = types.FirstOrDefault(x =>
                commandInterface.IsAssignableFrom(x) && x is { IsClass: true, IsAbstract: false });

            if (implementation == null) continue;

            var slashCommand = commandInterface.GetCustomAttribute<SlashCommand>()!;
            var slashCommandOptions = commandInterface.GetCustomAttributes<SlashCommandOption>();

            var builder = new SlashCommandBuilder()
                .WithName(slashCommand.Name)
                .WithDescription(slashCommand.Description)
                .WithNsfw(slashCommand.AgeRestriction)
                .WithDefaultPermission(slashCommand.DefaultPermission);

            foreach (var slashCommandOption in slashCommandOptions)
            {
                builder.AddOption(name: slashCommandOption.Name, description: slashCommandOption.Description,
                    type: slashCommandOption.Type, isRequired: slashCommandOption.Required);
            }

            var isGuild = slashCommand.Type is SlashCommandType.Guild;
            var isDirectMessage = slashCommand.Type is SlashCommandType.DirectMessage;

            if (slashCommand.Type is SlashCommandType.Global)
            {
                isGuild = true;
                isDirectMessage = true;
            }

            if (isDirectMessage)
                await _discord.Rest.CreateGlobalCommand(builder.Build());

            if (isGuild)
            {
                async void Action(SocketGuild guild)
                {
                    await _discord.Rest.CreateGuildCommand(builder.Build(), guild.Id);
                }

                _discord.Guilds.ToList().ForEach(Action);
            }

            _discord.SlashCommandExecuted += async (context) =>
            {
                if (context.Data.Name == slashCommand.Name)
                {
                    var commandService = _serviceProvider.GetService(commandInterface);

                    if (commandService is not IBaseSlashCommand command)
                        return;

                    try
                    {
                        await command.HandleAsync(context);
                    }catch (Exception e)
                    {
                        _logger.LogError(e, "Failed to execute command {command}", slashCommand.Name);
                    }
                }
            };
            
            _logger.LogInformation("Registered command {command} with {executor}", slashCommand.Name, implementation.Name);
        }
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Stopping Discord client");
        return _discord.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _discord.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        _discord.Dispose();
        GC.SuppressFinalize(this);
    }
}