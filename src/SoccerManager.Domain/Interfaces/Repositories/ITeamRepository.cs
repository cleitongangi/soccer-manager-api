using SoccerManager.Domain.DTOs;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Domain.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        Task AddAsync(TeamEntity entity);
        Task<int> DecreaseTeamBudgetAsync(long teamId, decimal value);
        Task<GetTeamDetailsDTO> GetTeamDetailsAsync(long teamId);
        Task<int> IncreaseTeamBudgetAsync(long teamId, decimal value);
        Task<int> UpdateAsync(TeamEntity entity);
    }
}
