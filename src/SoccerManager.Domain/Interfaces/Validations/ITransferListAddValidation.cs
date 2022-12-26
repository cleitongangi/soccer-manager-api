using FluentValidation;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Domain.Interfaces.Validations
{
    public interface ITransferListAddValidation : IValidator<TransferListEntity>
    {
    }
}
