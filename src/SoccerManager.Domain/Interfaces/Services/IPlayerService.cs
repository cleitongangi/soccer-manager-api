using FluentValidation.Results;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enum;

namespace SoccerManager.Domain.Interfaces.Services
{
    public interface IPlayerService
    {
        Task CreateInitialPlayersAsync(long teamId, PlayerPositionEnum positionEnum, short count);
        Task<ValidationResult> UpdatePlayerAsync(long teamId, PlayerEntity entity);
    }
}
