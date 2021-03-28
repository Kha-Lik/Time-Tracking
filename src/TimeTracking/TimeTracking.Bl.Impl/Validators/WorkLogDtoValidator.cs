using FluentValidation;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Validators
{
    public class WorkLogDtoValidator : AbstractValidator<WorkLogDto>
    {
        public WorkLogDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ActivityType)
                .IsInEnum();

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.TimeSpent)
                .NotEmpty()
                .NotNull();


            RuleFor(x => x.IssueId)
                .NotEmpty()
                .NotNull();
        }
    }
}