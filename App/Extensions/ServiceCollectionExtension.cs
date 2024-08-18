using System.Reflection;
using App.Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Extension method that adds a worker to the IServiceCollection.
    /// </summary>
    /// <typeparam name="TWorker">The type of the worker class.</typeparam>
    /// <param name="services">The IServiceCollection to add the worker to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddWorker<TWorker>(this IServiceCollection services)
        where TWorker : class, IHostedService
    {
        services.AddHostedService<TWorker>();
        return services;
    }

    /// <summary>
    /// Extension method that scans the executing assembly for interfaces with the SlashCommand attribute and registers them with their corresponding implementations in the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the commands to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();

        // Find all interfaces that are decorated with the SlashCommand attribute
        var commandInterfaces = types.Where(x => x.GetCustomAttribute<SlashCommand>() is not null && x.IsInterface);

        foreach (var commandInterface in commandInterfaces)
        {
            // Find the implementation that implements this interface
            var implementation = types.FirstOrDefault(x => commandInterface.IsAssignableFrom(x) && x is { IsClass: true, IsAbstract: false });

            if (implementation != null)
            {
                // Register the interface and its implementation in the service collection
                services.AddScoped(commandInterface, implementation);
            }
        }

        return services;
    }
}