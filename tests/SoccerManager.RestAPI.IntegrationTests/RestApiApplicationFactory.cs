using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SoccerManager.Infra.Data.Context;

namespace SoccerManager.RestAPI.IntegrationTests
{
    internal class RestApiApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<SoccerManagerDbContext>));
                services.AddDbContext<SoccerManagerDbContext>(options =>
                        options
                        .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=SoccerManager;Trusted_Connection=True;MultipleActiveResultSets=true")
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    );
            });

            return base.CreateHost(builder);
        }
    }
}
