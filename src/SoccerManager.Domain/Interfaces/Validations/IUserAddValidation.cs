using FluentValidation;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Domain.Interfaces.Validations
{
    public interface IUserAddValidation : IValidator<UserEntity>
    {
    }
}
