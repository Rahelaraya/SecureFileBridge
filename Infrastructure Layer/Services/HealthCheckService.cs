using Application_Layer.DTOs;
using Application_Layer.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure_Layer.Services;

/// <summary>
/// Service för att hantera health status för systemets komponenter.
/// </summary>
public class HealthCheckService : IHealthCheckService
{
    private const int FailureThreshold = 3;

    private readonly ILogger<HealthCheckService> _logger;

    private readonly Dictionary<string, ComponentHealth> _components = new()
    {
        ["FileRepository"] = new(),
        ["RabbitMqPublisher"] = new(),
        ["FilePollingService"] = new(),
        ["FileReaderService"] = new()
    };

    public HealthCheckService(
        ILogger<HealthCheckService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Hämtar aktuell health status för alla komponenter.
    /// </summary>
    public Task<HealthStatus> GetStatusAsync()
    {
        // Skapar en snapshot så originaldata inte ändras direkt
        var componentsSnapshot = _components.ToDictionary(
            component => component.Key,
            component => new ComponentHealth
            {
                IsHealthy = component.Value.IsHealthy,
                SuccessCount = component.Value.SuccessCount,
                FailureCount = component.Value.FailureCount,
                LastError = component.Value.LastError,
                LastCheckTime = component.Value.LastCheckTime
            });

        // Räknar hur många komponenter som inte är healthy
        var failedComponents =
            componentsSnapshot.Values.Count(x => !x.IsHealthy);

        var status = new HealthStatus
        {
            CheckTime = DateTime.Now,
            Components = componentsSnapshot,
            IsHealthy = failedComponents == 0,
            Message = failedComponents == 0
                ? "All components healthy"
                : $"{failedComponents} component(s) unhealthy"
        };

        _logger.LogInformation(
            "Health check completed: {Message}",
            status.Message);

        return Task.FromResult(status);
    }

    /// <summary>
    /// Registrerar en lyckad kontroll för en komponent.
    /// </summary>
    public void RecordSuccess(string component)
    {
        if (!_components.TryGetValue(component, out var health))
        {
            return;
        }

        health.SuccessCount++;
        health.FailureCount = 0;
        health.IsHealthy = true;
        health.LastError = null;
        health.LastCheckTime = DateTime.Now;

        _logger.LogInformation(
            "Component {Component} recorded success. Total successes: {Count}",
            component,
            health.SuccessCount);
    }

    /// <summary>
    /// Registrerar ett fel för en komponent.
    /// Efter 3 fel markeras komponenten som unhealthy.
    /// </summary>
    public void RecordFailure(
        string component,
        string error)
    {
        if (!_components.TryGetValue(component, out var health))
        {
            return;
        }

        health.FailureCount++;
        health.LastError = error;
        health.LastCheckTime = DateTime.Now;

        // Markera komponenten som unhealthy efter flera fel
        if (health.FailureCount >= FailureThreshold)
        {
            health.IsHealthy = false;

            _logger.LogError(
                "Component {Component} marked unhealthy. Failures: {Count}. Error: {Error}",
                component,
                health.FailureCount,
                error);
        }
        else
        {
            _logger.LogWarning(
                "Component {Component} recorded failure. Failures: {Count}. Error: {Error}",
                component,
                health.FailureCount,
                error);
        }
    }
}