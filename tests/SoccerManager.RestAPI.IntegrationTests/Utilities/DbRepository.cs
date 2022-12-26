using Microsoft.Extensions.DependencyInjection;
using SoccerManager.Domain.Entities;
using SoccerManager.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerManager.RestAPI.IntegrationTests.Utilities
{
    internal class DbRepository
    {
        private readonly RestApiApplicationFactory _application;

        public DbRepository(RestApiApplicationFactory application)
        {
            this._application = application;
        }

        internal async Task ResetDbAsync()
        {
            using var scope = _application.Services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<SoccerManagerDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

            // Remove all data
            dbContext.TeamPlayers.RemoveRange(dbContext.TeamPlayers);
            dbContext.Teams.RemoveRange(dbContext.Teams);
            dbContext.TransferLists.RemoveRange(dbContext.TransferLists);
            dbContext.Users.RemoveRange(dbContext.Users);
            await dbContext.SaveChangesAsync();

            await dbContext.Users.AddAsync(new UserEntity("tests@gmail.com", "$2a$11$mG4oyQG65hQeo4aE6Nb2feudcEt0bDoqfcm2Oz5MrUuRNhWyT52Jy", DateTime.Now));            

            await dbContext.SaveChangesAsync();
        }
    }
}
