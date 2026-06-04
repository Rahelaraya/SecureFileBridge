using Application_Layer.DTOs;
using Application_Layer.Interfaces;
using Infrastructure_Layer.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure_Layer.Services;

/// <summary>
/// Service för att övervaka filflödet.
/// </summary>
public class FlowMonitoringService : IFlowMonitoringService
{
    private readonly FilePollingSettings _settings;

    public FlowMonitoringService(
        IOptions<FilePollingSettings> options)
    {
        _settings = options.Value;
    }

    /// <summary>
    /// Hämtar status för filflödet.
    /// </summary>
    public FlowStatusResponse GetFlowStatus()
    {
        var sourcePath = _settings.SourcePath;

        
        var processedPath = Path.Combine(
            sourcePath,
            "processed");

        
        var sourceFiles = Directory.Exists(sourcePath)
            ? Directory.GetFiles(sourcePath)
            : [];

        var processedFiles = Directory.Exists(processedPath)
            ? Directory.GetFiles(processedPath)
            : [];

        var latestProcessed = processedFiles
            .Select(path => new FileInfo(path))
            .OrderByDescending(file => file.LastWriteTimeUtc)
            .FirstOrDefault();

        // Returnerar statusinformation
        return new FlowStatusResponse
        {
            Message = "File polling flow status",

            PollingStatus = "Running",

            SourceFilesWaiting = sourceFiles.Length,

            ProcessedFiles = processedFiles.Length,

            RabbitMqStatus = processedFiles.Any()
                ? "Files published to RabbitMQ"
                : "No published files",

            LatestProcessedFile = latestProcessed?.Name,

            LatestProcessedAt = latestProcessed?.LastWriteTimeUtc
        };
    }
}