using Application_Layer.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure_Layer.Services;

/// <summary>
/// Background service för heartbeat och telemetry monitoring.
/// </summary>
public class HeartbeatMonitoringService : BackgroundService
{
    private readonly ILogger<HeartbeatMonitoringService> _logger;
    private readonly IHeartbeatTelemetryService _telemetryService;

    private DateTime _lastHeartbeat = DateTime.Now;
    private int _heartbeatCount;

    public HeartbeatMonitoringService(
        ILogger<HeartbeatMonitoringService> logger,
        IHeartbeatTelemetryService telemetryService)
    {
        _logger = logger;
        _telemetryService = telemetryService;
    }

    /// <summary>
    /// Kör heartbeat monitoring i bakgrunden.
    /// </summary>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Heartbeat monitoring service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Uppdaterar heartbeat information
            _lastHeartbeat = DateTime.Now;
            _heartbeatCount++;

            // Hämtar minnesanvändning för processen
            var memoryUsage = GetMemoryUsage();

            _logger.LogInformation(
                "Heartbeat {Count} at {Time}",
                _heartbeatCount,
                _lastHeartbeat);

            // Skickar telemetry data
            _telemetryService.EmitHeartbeat(
                _heartbeatCount,
                memoryUsage);

            // Väntar 30 sekunder innan nästa heartbeat
            await Task.Delay(
                TimeSpan.FromSeconds(30),
                stoppingToken);
        }
    }

    /// <summary>
    /// Hämtar processens minnesanvändning i MB.
    /// </summary>
    private static long GetMemoryUsage()
    {
        using var process = Process.GetCurrentProcess();

        return process.WorkingSet64 / 1024 / 1024;
    }

    /// <summary>
    /// Hämtar senaste heartbeat tid.
    /// </summary>
    public DateTime GetLastHeartbeat() => _lastHeartbeat;

    /// <summary>
    /// Hämtar antal heartbeats.
    /// </summary>
    public int GetHeartbeatCount() => _heartbeatCount;

    /// <summary>
    /// Kontrollera om heartbeat fortfarande är healthy.
    /// </summary>
    public bool IsHealthy()
    {
        return DateTime.Now
            .Subtract(_lastHeartbeat)
            .TotalSeconds < 60;
    }
}