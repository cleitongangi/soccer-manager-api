using FluentValidation;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Validations;

namespace SoccerManager.Domain.Validations
{
    public class TeamUpdateValidation : AbstractValidator<TeamEntity>, ITeamUpdateValidation
    {
        public TeamUpdateValidation()
        {
            RuleFor(x => x.TeamId)
                .GreaterThan(0);

            RuleFor(x => x.TeamName)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.TeamCountry)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}
