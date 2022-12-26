using FluentValidation;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Validations
{
    public class UserAddValidation : AbstractValidator<UserEntity>, IUserAddValidation
    {
        public UserAddValidation(IUserRepository userRepository)
        {
            RuleFor(x => x.CreatedAt)
                .GreaterThan(new DateTime(1900, 1, 1)); // minimum date accepted by SQL Server

            RuleFor(x => x.Username)
                .MaximumLength(255)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(async (x, cancellation) => !await userRepository.HasAsync(x)).WithMessage("The username already exists.");

            RuleFor(x => x.Password)
                .MaximumLength(100)
                .NotEmpty();
        }
    }
}
