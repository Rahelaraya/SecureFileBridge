namespace API_Layer.Installers;

/// <summary>
/// Registrerar API-relaterade services.
/// </summary>
public class ApiInstaller : IInstaller
{
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
    }
}