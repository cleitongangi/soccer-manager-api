using FluentValidation.Results;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enum;
using SoccerManager.Domain.Interfaces.Data;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Services;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerUpdateValidation _playerUpdateValidation;

        public PlayerService(IUnitOfWork unitOfWork, IPlayerRepository playerRepository, IPlayerUpdateValidation playerUpdateValidation)
        {
            this._uow = unitOfWork;
            this._playerRepository = playerRepository;
            this._playerUpdateValidation = playerUpdateValidation;
        }

        public async Task CreateInitialPlayersAsync(long teamId, PlayerPositionEnum positionEnum, short count)
        {
            for (int i = 0; i < count; i++)
            {
                // Add new Player
                var newPlayer = PlayerEntity.Factory.NewInitialPlayer(positionEnum);
                await _playerRepository.AddAsync(newPlayer);
                await _uow.SaveChangesAsync();

                // Add TeamPlayer
                await _playerRepository.AddTeamPlayerAsync(TeamPlayerEntity.Factory.NewInitialPlayer(teamId, newPlayer.Id));
            }
        }

        public async Task<ValidationResult> UpdatePlayerAsync(long teamId, PlayerEntity entity)
        {
            var validationResult = await _playerUpdateValidation.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            // Update only if player belongs to user team
            var affectedRows = await _playerRepository.UpdateAsync(teamId, entity);
            if (affectedRows == 0)
            {
                validationResult.Errors.Add(new ValidationFailure("notfound", "Could not update, because the player does not exist or does not belong to your team."));
            }
            return validationResult;
        }
    }
}
