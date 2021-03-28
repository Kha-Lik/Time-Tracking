using FluentValidation;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Validators
{
    public class MilestoneDtoValidator : AbstractValidator<MilestoneDto>
    {
        public MilestoneDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.State)
                .IsInEnum();

            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull();


            RuleFor(x => x.DueDate)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .NotNull();
        }
    }
}