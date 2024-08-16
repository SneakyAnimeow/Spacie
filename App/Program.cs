global using IWorker = Microsoft.Extensions.Hosting.IHostedService;

using System.Reflection;
using App.Services.GameService;
using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        .AddInMemoryCollection([
            new KeyValuePair<string, string?>("Spacie:Discord:Token",
                Environment.GetEnvironmentVariable("DISCORD_TOKEN")),
        ])
        .Build();

    config.AddConfiguration(configuration);
});

builder.UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); });

builder.ConfigureServices(services =>
{
    services.AddScoped<IGameService, GameService>();
});

var app = builder.Build();

Log.Information("Starting Spacie v{Version}", Assembly.GetExecutingAssembly().GetName().Version);

app.Run();