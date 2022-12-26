using SoccerManager.Domain.Entities;

namespace SoccerManager.Domain.Interfaces.Repositories
{
    public interface IPlayerRepository
    {
        Task AddAsync(PlayerEntity entity);
        Task AddTeamPlayerAsync(TeamPlayerEntity entity);
        Task<int> DisableTeamPlayerAsync(long playerId, DateTime removedAt);
        Task<int> GetNextTeamPlayerSequenceAsync(long teamId, long playerId);
        Task<IEnumerable<PlayerEntity>> GetTeamPlayersAsync(long teamId);
        Task<bool> HasTeamPlayerActiveAsync(long teamId, long playerId);
        Task<int> UpdateAsync(long teamId, PlayerEntity entity);
        Task<int> UpdateMarketValueAsync(PlayerEntity entity);
    }
}
