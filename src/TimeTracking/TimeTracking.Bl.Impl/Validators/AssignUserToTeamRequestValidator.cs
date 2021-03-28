using FluentValidation;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Impl.Validators
{
    public class AssignUserToTeamRequestValidator : AbstractValidator<AssignUserToTeamRequest>
    {
        public AssignUserToTeamRequestValidator()
        {
            RuleFor(x => x.TeamId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty();
        }
    }
}