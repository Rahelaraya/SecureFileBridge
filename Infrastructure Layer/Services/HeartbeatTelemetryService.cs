using Application_Layer.Interfaces;
using System.Diagnostics;

namespace Infrastructure_Layer.Services;

/// <summary>
/// Service för att skicka heartbeat telemetry data.
/// </summary>
public class HeartbeatTelemetryService
    : IHeartbeatTelemetryService
{
    /// <summary>
    /// Skickar heartbeat metrics till OpenTelemetry.
    /// </summary>
    public void EmitHeartbeat(
        int heartbeatCount,
        long memoryUsage)
    {
        // Skapar en telemetry activity
        using var activity =
            new Activity("HeartbeatMetric");

        activity.Start();

     
        activity.SetTag(
            "heartbeat.count",
            heartbeatCount);

       
        activity.SetTag(
            "memory.usage.mb",
            memoryUsage);

        
        activity.SetTag(
            "timestamp",
            DateTime.Now.ToString("O"));
    }
}