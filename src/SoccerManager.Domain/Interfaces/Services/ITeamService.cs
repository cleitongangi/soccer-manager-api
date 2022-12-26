using FluentValidation.Results;

namespace SoccerManager.Domain.Interfaces.Services
{
    public interface ITeamService
    {
        Task CreateInitialTeamAndPlayersAsync(long teamId);
        Task<ValidationResult> UpdateAsync(long teamId, string teamName, string teamCountry);
    }
}
