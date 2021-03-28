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
                .NotNull();

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .NotNull();
        }
    }
}