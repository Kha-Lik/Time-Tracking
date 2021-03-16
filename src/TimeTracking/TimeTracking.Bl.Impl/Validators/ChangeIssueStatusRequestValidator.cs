using FluentValidation;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Impl.Validators
{
    public class ChangeIssueStatusRequestValidator:AbstractValidator<ChangeIssueStatusRequest>
    {
        public ChangeIssueStatusRequestValidator()
        {
            RuleFor(c => c.IssueId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Status)
                .NotEmpty()
                .IsInEnum();
        }
    }
}