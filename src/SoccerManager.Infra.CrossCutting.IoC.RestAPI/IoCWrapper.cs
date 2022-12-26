using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SoccerManager.Infra.CrossCutting.IoC.RestAPI
{
    public static class IoCWrapper
    {
        public static void RegisterIoC(IServiceCollection services, IConfiguration configuration)
        {
            var posterrDbConnectionString = configuration.GetConnectionString("PosterrDb");
            Data.RegisterIoC.Register(services, posterrDbConnectionString);
            Security.RegisterIoC.Register(services);
            Domain.RegisterIoC.Register(services);            
        }
    }
}
