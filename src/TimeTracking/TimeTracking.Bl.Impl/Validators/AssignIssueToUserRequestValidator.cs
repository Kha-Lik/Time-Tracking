using FluentValidation;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Impl.Validators
{
    public class AssignIssueToUserRequestValidator:AbstractValidator<AssignIssueToUserRequest>
    {
        public AssignIssueToUserRequestValidator()
        {
            RuleFor(x => x.IssueId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty()
                .NotNull();
 
        }
    }
}