using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace API_Layer.Installers;

/// <summary>
/// Registrerar OpenTelemetry för tracing, metrics och logging.
/// </summary>
public class OpenTelemetryInstaller : IInstaller
{
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenTelemetry()

            // Registrerar tracing
            .WithTracing(tracing =>
            {
                tracing                    
                    .AddAspNetCoreInstrumentation()                  
                    .AddHttpClientInstrumentation()

                    // Registrerar custom tracing sources
                    .AddSource("Infrastructure_Layer.Services.RabbitMqPublisher")
                    .AddSource("Infrastructure_Layer.Services.FilePollingService")
                    .AddSource("Infrastructure_Layer.Services.HeartbeatMonitoringService")
                    
                    .AddConsoleExporter();
            })

            // Registrerar metrics
            .WithMetrics(metrics =>
            {
                metrics                
                    .AddAspNetCoreInstrumentation()                   
                    .AddHttpClientInstrumentation()                    
                    .AddConsoleExporter();
            })

            // Registrerar logging
            .WithLogging(logging =>
            {               
                logging.AddConsoleExporter();
            });
    }
}