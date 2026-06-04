using Microsoft.OpenApi.Models;

namespace API_Layer.Installers;

/// <summary>
/// Registrerar Swagger/OpenAPI dokumentation.
/// </summary>
public class SwaggerInstaller : IInstaller
{
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrerar Swagger generator
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "File Bridge API",
                Version = "1.0.0",
                Description =
                    "File Bridge application for secure network file processing"
            });
        });
    }
}