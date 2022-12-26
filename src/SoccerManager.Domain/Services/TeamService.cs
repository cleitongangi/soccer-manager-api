using FluentValidation.Results;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enum;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Services;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerService _playerService;
        private readonly ITeamUpdateValidation _teamUpdateValidation;

        public TeamService(ITeamRepository teamRepository,
                           IPlayerService playerService,
                           ITeamUpdateValidation teamUpdateValidation)
        {
            this._teamRepository = teamRepository;
            this._playerService = playerService;
            this._teamUpdateValidation = teamUpdateValidation;
        }

        public async Task CreateInitialTeamAndPlayersAsync(long teamId)
        {
            // Add new team
            await _teamRepository.AddAsync(TeamEntity.Factory.NewInitialTeam(teamId));

            // Add players
            await _playerService.CreateInitialPlayersAsync(teamId: teamId, positionEnum: PlayerPositionEnum.Goalkeeper, count: 3);
            await _playerService.CreateInitialPlayersAsync(teamId: teamId, positionEnum: PlayerPositionEnum.Defender, count: 6);
            await _playerService.CreateInitialPlayersAsync(teamId: teamId, positionEnum: PlayerPositionEnum.Midfielder, count: 6);
            await _playerService.CreateInitialPlayersAsync(teamId: teamId, positionEnum: PlayerPositionEnum.Attacker, count: 5);
        }

        public async Task<ValidationResult> UpdateAsync(long teamId, string teamName, string teamCountry)
        {
            var entity = new TeamEntity(teamId, teamName, teamCountry);
            var validationResult = await _teamUpdateValidation.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (await _teamRepository.UpdateAsync(entity) == 0)
            {
                validationResult.Errors.Add(new ValidationFailure("notfound", "Team not found."));
            }
            return validationResult;
        }
    }
}
