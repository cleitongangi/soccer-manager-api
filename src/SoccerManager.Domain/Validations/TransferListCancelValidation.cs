using FluentValidation;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enum;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Validations
{
    public class TransferListCancelValidation : AbstractValidator<TransferListEntity>, ITransferListCancelValidation
    {
        public TransferListCancelValidation(ITransferListRepository transferListRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.SourceTeamId)
                .GreaterThan(0);

            RuleFor(x => x.UpdatedAt)
                .GreaterThan(new DateTime(1900, 1, 1)); // minimum date accepted by SQL Server

            RuleFor(x => x.StatusId)
                .Equal((short)TransferListStatusEnum.Canceled);

            RuleFor(x => x)
                .MustAsync(async (x, cancelation) => await transferListRepository.HasOpenByTransferIdAsync(x.Id, x.SourceTeamId))
                .WithMessage("This transfer does not belong to you or does not exist.")
                .OverridePropertyName("TransferId");
        }
    }
}
