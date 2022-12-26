using FluentValidation;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enum;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Validations
{
    public class TransferListAddValidation : AbstractValidator<TransferListEntity>, ITransferListAddValidation
    {
        public TransferListAddValidation(IPlayerRepository playerRepository, ITransferListRepository transferListRepository)
        {
            RuleFor(x => x.PlayerId)
                .GreaterThan(0);

            RuleFor(x => x.CreatedAt)
                .GreaterThan(new DateTime(1900, 1, 1)); // minimum date accepted by SQL Server

            RuleFor(x => x.SourceTeamId)
                .GreaterThan(0);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.UpdatedAt)
                .GreaterThan(new DateTime(1900, 1, 1)); // minimum date accepted by SQL Server

            RuleFor(x => x.StatusId)
                .Equal((short)TransferListStatusEnum.Open);

            RuleFor(x => x)
                .MustAsync(async (x, cancelation) => await playerRepository.HasTeamPlayerActiveAsync(x.SourceTeamId, x.PlayerId))                
                .WithMessage("The player does not belong to your team.")
                .OverridePropertyName("PlayerId");

            RuleFor(x => x)
                .MustAsync(async (x, cancelation) => !await transferListRepository.HasOpenAsync(x.SourceTeamId, x.PlayerId))
                .WithMessage("The player is already on the transfer list.")
                .OverridePropertyName("PlayerId");
        }
    }
}
