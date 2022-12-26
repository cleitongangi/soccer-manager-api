using FluentValidation;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Validations
{
    public class PlayerUpdateValidation : AbstractValidator<PlayerEntity>, IPlayerUpdateValidation
    {
        public PlayerUpdateValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.FirstName)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.Country)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.UpdatedAt)
                .GreaterThan(new DateTime(1900, 1, 1));
        }
    }
}
