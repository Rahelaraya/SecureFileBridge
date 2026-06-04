using Application_Layer.Interfaces;
using Infrastructure_Layer.Configuration;
using Infrastructure_Layer.Repositories;
using Infrastructure_Layer.Services;

namespace API_Layer.Installers;

/// <summary>
/// Registrerar Infrastructure Layer services.
/// </summary>
public class InfrastructureInstaller : IInstaller
{
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrerar FilePolling settings från appsettings.json
        services.Configure<FilePollingSettings>(
            configuration.GetSection("FilePolling"));

        // Registrerar RabbitMQ settings från appsettings.json
        services.Configure<RabbitMqSettings>(
            configuration.GetSection("RabbitMQ"));

        // Registrerar repositories
        services.AddScoped<IFileRepository, FileRepository>();

        // Registrerar services
        services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
        services.AddScoped<IFileReaderService, FileReaderService>();
        services.AddScoped<IRetryPolicyService, RetryPolicyService>();
        services.AddScoped<IHealthCheckService, HealthCheckService>();
        services.AddScoped<IFlowMonitoringService, FlowMonitoringService>();

        // Registrerar heartbeat services som singleton
        services.AddSingleton<IHeartbeatTelemetryService, HeartbeatTelemetryService>();
        services.AddSingleton<HeartbeatMonitoringService>();

        // Registrerar background services
        services.AddHostedService(sp =>
            sp.GetRequiredService<HeartbeatMonitoringService>());

        services.AddHostedService<FilePollingService>();
    }
}