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
}