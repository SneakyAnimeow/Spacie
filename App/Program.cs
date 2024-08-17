global using IWorker = Microsoft.Extensions.Hosting.IHostedService;
using App.Extensions;
using App.Services.AppInfoService;
using App.Services.DiscordClientService;
using App.Services.GameService;
using App.Workers;
using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

Env.Load(".env");

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true,
            reloadOnChange: true)
        .AddDotNetEnv(".env")
        .Build();

    config.AddConfiguration(configuration);
});

builder.UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); });

builder.ConfigureServices(services =>
{
    services.AddScoped<IAppInfoService, AppInfoService>();
    services.AddScoped<IGameService, GameService>();
    services.AddScoped<IDiscordClientService, DiscordClientService>();
    services.AddWorker<DiscordWorker>();
});

var app = builder.Build();

using var scope = app.Services.CreateScope();

var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
var appInfoService = scope.ServiceProvider.GetRequiredService<IAppInfoService>();

logger.LogInformation("Starting {appName}", await appInfoService.GetFullNameAsync());

app.Run();