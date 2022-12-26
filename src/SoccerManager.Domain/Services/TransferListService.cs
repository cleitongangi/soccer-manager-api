using FluentValidation.Results;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Data;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Services;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Services
{
    public class TransferListService : ITransferListService
    {
        private readonly IUnitOfWork _uow;
        private readonly ITransferListRepository _transferListRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITransferListAddValidation _transferListAddValidation;
        private readonly ITransferListCancelValidation _transferListCancelValidation;
        private readonly ITransferListBuyValidation _transferListBuyValidation;

        public TransferListService(IUnitOfWork unitOfWork,
                                   ITransferListRepository transferListRepository,
                                   IPlayerRepository playerRepository,
                                   ITeamRepository teamRepository,
                                   ITransferListAddValidation transferListAddValidation,
                                   ITransferListCancelValidation transferListCancelValidation,
                                   ITransferListBuyValidation transferListBuyValidation)
        {
            this._uow = unitOfWork;
            this._transferListRepository = transferListRepository;
            this._playerRepository = playerRepository;
            this._teamRepository = teamRepository;
            this._transferListAddValidation = transferListAddValidation;
            this._transferListCancelValidation = transferListCancelValidation;
            this._transferListBuyValidation = transferListBuyValidation;
        }

        public async Task<(ValidationResult, long?)> AddAsync(long teamId, long playerId, decimal price)
        {
            var entity = TransferListEntity.Factory.NewTransfer(teamId, playerId, price);
            var validationResult = await _transferListAddValidation.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return (validationResult, null);
            }

            await _transferListRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return (validationResult, entity.Id);
        }

        public async Task<ValidationResult> RemoveAsync(long teamId, long transferId)
        {
            var entity = TransferListEntity.Factory.CreateToCancelTransfer(transferId, teamId);
            var validationResult = await _transferListCancelValidation.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            await _transferListRepository.CancelAsync(entity);
            await _uow.SaveChangesAsync();

            return validationResult;
        }

        public async Task<ValidationResult> BuyAsync(long transferId, long targetTeamId)
        {
            var entity = TransferListEntity.Factory.CreateToBuy(transferId, targetTeamId);
            var validationResult = await _transferListBuyValidation.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var transferData = await _transferListRepository.GetDataToTransferAsync(transferId);
            if (transferData == null)
            {
                validationResult.Errors.Add(new ValidationFailure("", "This transfer does not exist or has already been closed."));
                return validationResult;
            }

            try
            {
                await _uow.BeginTransactionAsync();

                // Debit new team account balance
                var rowsAffected = await _teamRepository.DecreaseTeamBudgetAsync(targetTeamId, transferData.Price);
                if (rowsAffected == 0)
                {
                    await _uow.RollbackAsync();
                    validationResult.Errors.Add(new ValidationFailure("TransferId", "You don't have enough funds to buy the player."));
                    return validationResult;
                }

                // Credit balance on the old team account
                await _teamRepository.IncreaseTeamBudgetAsync(transferData.SourceTeamId, transferData.Price);

                // Remove player from old team                
                await _playerRepository.DisableTeamPlayerAsync(transferData.PlayerId, DateTime.Now);

                // Add player to new team
                var sequence = await _playerRepository.GetNextTeamPlayerSequenceAsync(targetTeamId, transferData.PlayerId);
                var newTeamPlayer = TeamPlayerEntity.Factory.CreateToTransfer(targetTeamId, transferData.PlayerId, sequence);
                await _playerRepository.AddTeamPlayerAsync(newTeamPlayer);

                // Update transfer list
                await _transferListRepository.TransferAsync(entity);

                // Increase Player Market Value and Update
                var player = PlayerEntity.Factory.CreateForTransfer(transferData.PlayerId, transferData.Price);
                player.IncreaseMarketValue();
                await _playerRepository.UpdateMarketValueAsync(player);

                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }

            return validationResult;
        }
    }
}
