using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoccerManager.Infra.Data.Context;

namespace SoccerManager.Infra.CrossCutting.IoC.RestAPI
{
    public static class MigrationManager
    {
        public static void ApplyMigration(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ISoccerManagerDbContext>();
            appContext.Database.Migrate();
        }
    }
}
