using FluentValidation;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enum;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Validations
{
    public class TransferListBuyValidation : AbstractValidator<TransferListEntity>, ITransferListBuyValidation
    {
        public TransferListBuyValidation(ITransferListRepository transferListRepository, ITeamRepository teamRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.TargetTeamId)
                .GreaterThan(0);

            RuleFor(x => x.UpdatedAt)
                .GreaterThan(new DateTime(1900, 1, 1)); // minimum date accepted by SQL Server

            RuleFor(x => x.TransferedAt)
                .GreaterThan(new DateTime(1900, 1, 1)); // minimum date accepted by SQL Server

            RuleFor(x => x.StatusId)
                .Equal((short)TransferListStatusEnum.Transferred);

            RuleFor(x => x)
                .MustAsync(async (x, cancelation) => await transferListRepository.CanBuyAsync(x.Id, x.TargetTeamId ?? 0))
                .WithMessage("'Transfer Id' invalid.")
                .OverridePropertyName("TransferId")
                .When(x => x.TargetTeamId.HasValue);
        }
    }
}
