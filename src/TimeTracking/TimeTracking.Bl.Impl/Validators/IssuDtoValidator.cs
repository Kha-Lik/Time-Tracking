using FluentValidation;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Validators
{
    public class IssueDtoValidator : AbstractValidator<IssueDto>
    {
        public IssueDtoValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.Status)
                .IsInEnum();
            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.ReportedByUserId)
                .NotEmpty()
                .NotNull();
        }

    }
}