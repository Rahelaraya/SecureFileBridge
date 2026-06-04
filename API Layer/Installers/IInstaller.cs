namespace API_Layer.Installers;

public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration configuration);
}