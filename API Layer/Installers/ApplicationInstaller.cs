using Application_Layer.Features.Files.Queries;

namespace API_Layer.Installers;

/// <summary>
/// Registrerar Application Layer services.
/// </summary>
public class ApplicationInstaller : IInstaller
{
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrerar MediatR handlers från Application Layer
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(GetAllFilesQuery).Assembly);
        });
    }
}