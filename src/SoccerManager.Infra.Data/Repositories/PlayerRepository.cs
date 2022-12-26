using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Infra.Data.Context;

namespace SoccerManager.Infra.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ISoccerManagerDbContext _db;

        public PlayerRepository(ISoccerManagerDbContext soccerManagerDbContext)
        {
            _db = soccerManagerDbContext;
        }

        public async Task AddAsync(PlayerEntity entity)
        {
            await _db.Players.AddAsync(entity);
        }

        /// <summary>
        /// Update only if player belongs to teamId
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long teamId, PlayerEntity entity)
        {
            var sql = @"update Players
                set
                     FirstName = @FirstName
                    ,LastName = @LastName
                    ,Country = @Country
                    ,UpdatedAt = @UpdatedAt
                where Id = @Id
                    and exists(select 1 from TeamPlayers pt1 
		                    where pt1.PlayerId = Players.Id 
		                    and pt1.TeamId = @teamId
                            and pt1.Active = 1)";

            var param = new
            {
                teamId,
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Country,
                entity.UpdatedAt
            };
            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, param);
        }

        public async Task<int> UpdateMarketValueAsync(PlayerEntity entity)
        {
            var sql = @"update Players
                set 
                     MarketValue = @MarketValue
                    ,UpdatedAt = @UpdatedAt
                where Id = @Id";
                        
            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, entity, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task AddTeamPlayerAsync(TeamPlayerEntity entity)
        {
            await _db.TeamPlayers.AddAsync(entity);
        }

        public async Task<int> DisableTeamPlayerAsync(long playerId, DateTime removedAt)
        {
            var sql = @"update TeamPlayers
                set 
	                 Active = 0
	                ,RemovedAt = @RemovedAt
                where PlayerId = @PlayerId
	                and Active = 1";

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, new { playerId, removedAt }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<bool> HasTeamPlayerActiveAsync(long teamId, long playerId)
        {
            return await _db.TeamPlayers
                .AnyAsync(x => x.PlayerId.Equals(playerId)
                    && x.TeamId.Equals(teamId)
                    && x.Active.Equals(true));
        }

        public async Task<IEnumerable<PlayerEntity>> GetTeamPlayersAsync(long teamId)
        {
            var sql = @"select 
                     Id
	                ,FirstName
	                ,LastName
                    ,Country
	                ,Age
	                ,MarketValue
                from TeamPlayers tp
                    inner join Players p on tp.PlayerId = p.Id
                where tp.TeamId = @teamId";

            return await _db.Database.GetDbConnection()
                .QueryAsync<PlayerEntity>(sql, new { teamId });
        }

        public async Task<int> GetNextTeamPlayerSequenceAsync(long teamId, long playerId)
        {
            var sql = "select isnull(max(sequence),0) + 1 from TeamPlayers where TeamId = @TeamId and PlayerId = @PlayerId";

            return await _db.Database.GetDbConnection()
                .QueryFirstAsync<int>(sql, new { teamId, playerId }, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }
    }
}
