namespace App.Services.AppInfoService;


/// <summary>
/// Represents a service for retrieving application information.
/// </summary>
public interface IAppInfoService
{
    /// <summary>
    /// Retrieves the full name of the application asynchronously.
    /// </summary>
    /// <returns>The full name of the application.</returns>
    Task<string> GetFullNameAsync();

    /// <summary>
    /// Retrieves the name of the application asynchronously.
    /// </summary>
    /// <returns>The name of the application.</returns>
    Task<string> GetNameAsync();

    /// <summary>
    /// Retrieves the version number of the current application.
    /// </summary>
    /// <returns>
    /// The version number of the current application, or null if the version number is not available.
    /// </returns>
    Task<string?> GetVersionAsync();

    /// <summary>
    /// Retrieves the version number of the installed .NET asynchronously.
    /// </summary>
    /// <returns>
    /// The version number of the installed .NET , or null if the version number is not available.
    /// </returns>
    Task<string> GetDotnetVersionAsync();
}