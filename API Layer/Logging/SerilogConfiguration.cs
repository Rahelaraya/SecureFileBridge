using Serilog;
using Serilog.Formatting.Compact;

namespace API_Layer.Logging;

/// <summary>
/// Konfigurerar Serilog för logging.
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    /// Registrerar console, txt och json logging.
    /// </summary>
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                "logs/bridge-application-.txt",
                rollingInterval: RollingInterval.Day)

            .WriteTo.File(
                new CompactJsonFormatter(),
                "logs/bridge-application-.json",
                rollingInterval: RollingInterval.Day)

            .CreateLogger();
    }
}