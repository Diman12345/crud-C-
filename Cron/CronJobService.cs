public class CronJobService : IHostedService, IDisposable
{
    private readonly ILogger<CronJobService> _logger;
    private Timer _timer;

    public CronJobService(ILogger<CronJobService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cron Job Service is starting.");

        // Set timer untuk menjalankan tugas setiap 5 detik (ubah sesuai kebutuhan)
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        // Logika yang akan dijalankan
        _logger.LogInformation("Cron Job is working.");
        
        // Tambahkan logika tugas yang ingin dijalankan di sini
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cron Job Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
