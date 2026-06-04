using Application_Layer.Interfaces;
using Infrastructure_Layer.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Layer.Controllers;

/// <summary>
/// Hanterar health checks och heartbeat-status.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthCheckService _healthCheckService;
    private readonly HeartbeatMonitoringService _heartbeatService;

    public HealthController(
        IHealthCheckService healthCheckService,
        HeartbeatMonitoringService heartbeatService)
    {
        _healthCheckService = healthCheckService;
        _heartbeatService = heartbeatService;
    }

    /// <summary>
    /// Hämtar systemets health status.
    /// </summary>
    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        var health = await _healthCheckService.GetStatusAsync();
        var heartbeat = _heartbeatService.GetHeartbeatCount();
        var isHealthy = _heartbeatService.IsHealthy();

        return Ok(new
        {
            health.IsHealthy,
            health.CheckTime,
            health.Message,
            Heartbeats = heartbeat,
            HeartbeatHealthy = isHealthy,
            Components = health.Components
        });
    }

    /// <summary>
    /// Hämtar heartbeat-information.
    /// </summary>
    [HttpGet("heartbeat")]
    public IActionResult GetHeartbeat()
    {
        var lastHeartbeat = _heartbeatService.GetLastHeartbeat();
        var count = _heartbeatService.GetHeartbeatCount();
        var healthy = _heartbeatService.IsHealthy();

        return Ok(new
        {
            LastHeartbeat = lastHeartbeat,
            HeartbeatCount = count,
            IsHealthy = healthy,
            Timestamp = DateTime.Now
        });
    }
}