using System.Reflection;
using App.Data.Sqlite.Repositories;
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
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Get the current assembly
        var assembly = Assembly.GetExecutingAssembly();

        // Iterate over all types in the assembly
        foreach (var type in assembly.GetTypes())
        {
            // Get all interfaces implemented by the type
            var interfaces = type.GetInterfaces();

            // Check if the type implements any interface that inherits from IRepositoryDummyInterface
            foreach (var @interface in interfaces)
            {
                // Check if the interface inherits from IRepository<T>
                var repositoryInterface = @interface.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>));

                // Check if IRepository<T> inherits from IRepositoryDummyInterface
                if (!(repositoryInterface != null && typeof(IRepositoryDummyInterface).IsAssignableFrom(repositoryInterface))) 
                    continue;
                
                // Register the interface and its implementation in the service collection
                services.AddScoped(@interface, type);
            }
        }

        return services;
    }
}