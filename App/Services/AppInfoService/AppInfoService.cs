using System.Reflection;

namespace App.Services.AppInfoService;

/// <inheritdoc cref="IAppInfoService"/>
public class AppInfoService : IAppInfoService
{
    public async Task<string> GetFullNameAsync()
    {
        var version = await GetVersionAsync();
        var name = await GetNameAsync();
        
        return version is not null ? $"{name} v{version}" : name;
    }

    public Task<string> GetNameAsync()
    {
        return Task.FromResult("Spacie");
    }

    public Task<string?> GetVersionAsync()
    {
        return Task.FromResult(Assembly.GetExecutingAssembly().GetName().Version?.ToString());
    }

    public Task<string> GetDotnetVersionAsync()
    {
        return Task.FromResult(Environment.Version.ToString());
    }
}