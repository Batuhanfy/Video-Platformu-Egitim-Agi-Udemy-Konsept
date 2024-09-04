using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class BackgroundWorker : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<BackgroundWorker> _logger;

    public BackgroundWorker(IBackgroundTaskQueue taskQueue, ILogger<BackgroundWorker> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(stoppingToken);

            try
            {
                await workItem(stoppingToken);
                _logger.LogInformation("Başarıyla gerçekleştirildi. ");
                Console.WriteLine("Başarıyla gerçekleştirildi.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hata oluştu.");
                Console.WriteLine("Hata oluştu.");


            }
        }
    }
}
