using System.Reflection;

namespace App.Services.AppInfoService;

/// <inheritdoc cref="IAppInfoService"/>
public class AppInfoService : IAppInfoService
{
    public async Task<string> GetFullNameAsync()
    {
        var version = await GetVersionAsync();
        
        return version is not null ? $"Spacie v{version}" : "Spacie";
    }

    public Task<string?> GetVersionAsync()
    {
        return Task.FromResult(Assembly.GetExecutingAssembly().GetName().Version?.ToString());
    }
}