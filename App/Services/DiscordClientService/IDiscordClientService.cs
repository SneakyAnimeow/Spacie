namespace App.Services.DiscordClientService;

/// <summary>
/// Represents a Discord client service.
/// </summary>
public interface IDiscordClientService : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Starts the Discord client.
    /// </summary>
    /// <returns>Returns a task representing the asynchronous operation. The task result is a boolean value indicating whether the start operation was successful or not.</returns>
    Task<bool> StartAsync();

    /// <summary>
    /// Stops the Discord client.
    /// </summary>
    /// <returns>Returns a task representing the asynchronous operation.</returns>
    Task StopAsync();
}