using System.Reflection;

namespace API_Layer.Installers;

/// <summary>
/// Extension methods för att registrera alla installers automatiskt.
/// </summary>
public static class InstallerExtensions
{
    /// <summary>
    /// Söker efter alla klasser som implementerar IInstaller
    /// och registrerar deras services.
    /// </summary>
    public static void InstallServicesInAssembly(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        var installers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type =>
                typeof(IInstaller).IsAssignableFrom(type) &&
                !type.IsInterface &&
                !type.IsAbstract)

           
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>();

       
        foreach (var installer in installers)
        {
            installer.InstallServices(
                services,
                configuration);
        }
    }
}