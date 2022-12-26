using FluentValidation.Results;

namespace SoccerManager.Domain.Interfaces.Services
{
    public interface ITransferListService
    {
        Task<(ValidationResult, long?)> AddAsync(long teamId, long playerId, decimal price);
        Task<ValidationResult> BuyAsync(long transferId, long targetTeamId);
        Task<ValidationResult> RemoveAsync(long teamId, long transferId);
    }
}
