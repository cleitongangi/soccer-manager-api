using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SoccerManager.Domain.Interfaces.Data;
using SoccerManager.Infra.Data.Context;
using SoccerManager.Infra.Data.Repositories;

namespace SoccerManager.Infra.Data
{
    public static class RegisterIoC
    {
        public static readonly LoggerFactory MyLoggerFactory = new(new[] { new DebugLoggerProvider() });

        public static void Register(IServiceCollection services, string posterrDbConnectionString)
        {
            #region Register DBContext            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISoccerManagerDbContext, SoccerManagerDbContext>();

            services.AddDbContext<SoccerManagerDbContext>(options =>
            {
                options
                    .UseSqlServer(posterrDbConnectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseLoggerFactory(MyLoggerFactory);
            });
            #endregion Register DBContext


            #region Register all repositories
            var repositoriesAssembly = typeof(UserRepository).Assembly;
            var repositoriesRegistrations =
                from type in repositoriesAssembly.GetExportedTypes()
                where type.Namespace == "SoccerManager.Infra.Data.Repositories"
                where type.GetInterfaces().Any()
                select new { Interface = type.GetInterfaces().FirstOrDefault(), Implementation = type };

            foreach (var reg in repositoriesRegistrations)
                services.AddScoped(reg.Interface, reg.Implementation);
            #endregion Register all repositories
        }
    }
}
