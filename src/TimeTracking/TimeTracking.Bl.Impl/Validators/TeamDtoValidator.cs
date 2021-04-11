using FluentValidation;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Validators
{
    public class TeamDtoValidator : AbstractValidator<TeamDto>
    {
        public TeamDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.MembersCount)
                .NotEmpty()
                .InclusiveBetween(1,100)
                .NotNull();

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .NotNull();
        }
    }
}