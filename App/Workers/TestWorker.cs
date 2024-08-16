using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Workers;

public class TestWorker : IWorker, IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TestWorker> _logger;
    
    private Timer? _timer;

    public TestWorker(IConfiguration configuration, ILogger<TestWorker> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    public void DoWork(object? state)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_timer != null)
        {
            await _timer.DisposeAsync();
        }

        _timer = null;
        GC.SuppressFinalize(this);
    }
}