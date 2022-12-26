using Dapper;
using Microsoft.EntityFrameworkCore;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enum;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Infra.Data.Context;
using SoccerManager.Domain.Core.Extensions;
using SoccerManager.Domain.Core.Pagination;
using SoccerManager.Domain.Interfaces.GlobalSettings;
using Microsoft.EntityFrameworkCore.Storage;

namespace SoccerManager.Infra.Data.Repositories
{
    public class TransferListRepository : ITransferListRepository
    {
        private readonly ISoccerManagerDbContext _db;
        private readonly IConfigSettings _configSettings;

        public TransferListRepository(ISoccerManagerDbContext soccerManagerDbContext, IConfigSettings configSettings)
        {
            this._db = soccerManagerDbContext;
            this._configSettings = configSettings;
        }

        public async Task AddAsync(TransferListEntity entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task<int> CancelAsync(TransferListEntity entity)
        {
            var sql = @"update TransferList
                    set
	                     UpdatedAt = @UpdatedAt
	                    ,StatusId = @StatusId
                    where Id = @id	
	                    and StatusId = 1"; // Check StatusId to ensure it won't be canceled twice, can happen in concurrency scenarios.

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, entity);
        }

        public async Task<int> TransferAsync(TransferListEntity entity)
        {
            var sql = @"update TransferList
                set 
	                 TargetTeamId = @TargetTeamId
	                ,StatusId = @StatusId
	                ,TransferedAt = @TransferedAt
	                ,UpdatedAt = @TransferedAt
                where Id = @id
                    and StatusId = 1"; // Check StatusId to ensure it won't be transfered twice, can happen in concurrency scenarios.

            return await _db.Database.GetDbConnection()
                .ExecuteAsync(sql, entity, _db.Database?.CurrentTransaction?.GetDbTransaction());
        }

        public async Task<bool> HasOpenAsync(long sourceTeamId, long playerId)
        {
            return await _db.TransferLists
                .AnyAsync(x => x.SourceTeamId.Equals(sourceTeamId)
                    && x.PlayerId.Equals(playerId)
                    && x.StatusId.Equals(1));
        }

        public async Task<bool> HasOpenByTransferIdAsync(long id, long sourceTeamId)
        {
            return await _db.TransferLists
                .AnyAsync(x => x.Id.Equals(id)
                    && x.SourceTeamId.Equals(sourceTeamId)
                    && x.StatusId.Equals((short)TransferListStatusEnum.Open));
        }

        public async Task<bool> CanBuyAsync(long id, long targetTeamId)
        {
            return await _db.TransferLists
                .AnyAsync(x => x.Id.Equals(id)
                    && !x.SourceTeamId.Equals(targetTeamId)
                    && x.StatusId.Equals(1));
        }

        public async Task<PagedResult<TransferListEntity>> GetAsync(long? teamId, int page = 1)
        {
            var query = _db.TransferLists
                .Select(x => new TransferListEntity
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Price = x.Price,
                    Player = new PlayerEntity
                    {
                        Id = x.Player.Id,
                        FirstName = x.Player.FirstName,
                        LastName = x.Player.LastName,
                        Country = x.Player.Country,
                        Age = x.Player.Age,
                        PlayerPosition = new PlayerPositionEntity
                        {
                            PositionName = x.Player.PlayerPosition.PositionName
                        }
                    },
                    SourceTeam = new TeamEntity
                    {
                        TeamId = x.SourceTeam.TeamId,
                        TeamName = x.SourceTeam.TeamName,
                        TeamCountry = x.SourceTeam.TeamCountry
                    }
                });

            if (teamId.HasValue)
            {
                query = query.Where(x => x.SourceTeam.TeamId == teamId.Value);
            }

            var pageSize = _configSettings.DefaultPaginationPageSize;
            var skip = (page - 1) * pageSize;
            var data = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<TransferListEntity>(data, page, pageSize);
        }

        public async Task<TransferListEntity?> GetAsync(long? transferId)
        {
            return await _db.TransferLists
                .Select(x => new TransferListEntity
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Price = x.Price,
                    Player = new PlayerEntity
                    {
                        Id = x.Player.Id,
                        FirstName = x.Player.FirstName,
                        LastName = x.Player.LastName,
                        Country = x.Player.Country,
                        Age = x.Player.Age,
                        PlayerPosition = new PlayerPositionEntity
                        {
                            PositionName = x.Player.PlayerPosition.PositionName
                        }
                    },
                    SourceTeam = new TeamEntity
                    {
                        TeamId = x.SourceTeam.TeamId,
                        TeamName = x.SourceTeam.TeamName,
                        TeamCountry = x.SourceTeam.TeamCountry
                    }
                })
                .FirstOrDefaultAsync(x=>x.Id == transferId);
        }

        public async Task<TransferListEntity?> GetDataToTransferAsync(long id)
        {
            return await _db.TransferLists
                .Where(x => x.Id.Equals(id)
                    && x.StatusId.Equals(1))
                .Select(x => new TransferListEntity
                {
                    Id = x.Id,
                    PlayerId = x.PlayerId,
                    Price = x.Price,
                    SourceTeamId = x.SourceTeamId
                })
                .FirstOrDefaultAsync();
        }
    }
}
