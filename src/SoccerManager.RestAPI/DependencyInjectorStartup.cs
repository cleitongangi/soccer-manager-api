using SoccerManager.Domain.Interfaces.GlobalSettings;
using SoccerManager.RestAPI.GlobalSettings;

namespace SoccerManager.RestAPI
{
    public static class DependencyInjectorStartup
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfigSettings, ConfigSettings>();

            SoccerManager.Infra.CrossCutting.IoC.RestAPI.IoCWrapper.RegisterIoC(services, configuration);
        }
    }
}
