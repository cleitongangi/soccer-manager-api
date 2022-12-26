using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SoccerManager.Domain.DTOs;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Infra.Data.Context;

namespace SoccerManager.Infra.Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ISoccerManagerDbContext _db;

        public TeamRepository(ISoccerManagerDbContext soccerManagerDbContext)
        {
            _db = soccerManagerDbContext;
        }

        public async Task AddAsync(TeamEntity entity)
        {
            await _db.Teams.AddAsync(entity);
        }
        
        public async Task<GetTeamDetailsDTO> GetTeamDetailsAsync(long teamId)
        {
            var sql = @"select
                     t.TeamId
                    ,t.TeamName
                    ,t.TeamCountry
                    ,(select sum(p1.MarketValue) 
	                    from TeamPlayers tp1 
		                    inner join Players p1 on p1.Id = tp1.PlayerId
	                    where tp1.TeamId = t.TeamId 
		                    and tp1.Active = 1
                     ) as TeamValue
                from Teams t
                where t.TeamId = @teamId";

            return await _db.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<GetTeamDetailsDTO>(sql, new { teamId });
        }

        public async Task<int> UpdateAsync(TeamEntity entity)
        {
            var sql = @"update Teams
                set 
                    TeamName = @TeamName
                    ,TeamCountry = @TeamCountry                
                where TeamId = @TeamId";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, entity);
        }

        public async Task<int> IncreaseTeamBudgetAsync(long teamId, decimal value)
        {            
            var sql = @"update Teams
                set 
                    Budget = Budget + @value                    
                where TeamId = @TeamId";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, new {teamId,value }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<int> DecreaseTeamBudgetAsync(long teamId, decimal value)
        {
            var sql = @"update Teams
                set 
                    Budget = Budget - @value                    
                where TeamId = @TeamId
                    and Budget >= @value"; // To ensure that the team has Budget. This is important in concurrency scenarios

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, new { teamId, value }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }
    }
}
