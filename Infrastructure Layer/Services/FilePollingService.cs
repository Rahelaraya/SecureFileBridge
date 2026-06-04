using Application_Layer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure_Layer.Services;

/// <summary>
/// Background service som kontrollerar om nya filer finns att behandla.
/// </summary>
public class FilePollingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FilePollingService> _logger;

    public FilePollingService(
        IServiceScopeFactory scopeFactory,
        ILogger<FilePollingService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <summary>
    /// Körs i bakgrunden och kontrollerar filer med jämna intervall.
    /// </summary>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "File polling service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var fileRepository =
                scope.ServiceProvider
                    .GetRequiredService<IFileRepository>();

            var publisher =
                scope.ServiceProvider
                    .GetRequiredService<IMessagePublisher>();

            // Hämtar alla filer från source folder
            var files = await fileRepository.GetAllAsync();

            foreach (var file in files)
            {
                try
                {
                    // Publicerar filen till RabbitMQ
                    await publisher.PublishFileMessageAsync(file);

                    _logger.LogInformation(
                        "File published to RabbitMQ: {FileName}",
                        file.FileName);

                    // Skapar processed-mapp om den inte finns
                    var processedFolder = Path.Combine(
                        Path.GetDirectoryName(file.FilePath)!,
                        "processed");

                    Directory.CreateDirectory(processedFolder);

                    // Skapar destination path för filen
                    var processedPath = Path.Combine(
                        processedFolder,
                        file.FileName);

                    // Tar bort gammal fil om den redan finns
                    if (File.Exists(processedPath))
                    {
                        File.Delete(processedPath);
                    }

                    // Flyttar filen till processed-mappen
                    File.Move(
                        file.FilePath,
                        processedPath);

                    _logger.LogInformation(
                        "File moved to processed folder: {FileName}",
                        file.FileName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to publish file: {FileName}",
                        file.FileName);
                }
            }

            // Väntar 10 sekunder innan nästa kontroll
            await Task.Delay(
                TimeSpan.FromSeconds(10),
                stoppingToken);
        }
    }
}