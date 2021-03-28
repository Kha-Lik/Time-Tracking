using FluentValidation;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Impl.Validators
{
    public class UpdateWorkLogStatusRequestValidator : AbstractValidator<UpdateWorkLogStatusRequest>
    {
        public UpdateWorkLogStatusRequestValidator()
        {
            RuleFor(s => s.IsApproved)
                .NotNull();

            RuleFor(d => d.WorkLogId)
                .NotEmpty();

            RuleFor(d => d.Description)
                .NotNull()
                .NotEmpty()
                .When(e => !e.IsApproved)
                .WithErrorCode(ErrorCode.ClientError.ToString())
                .WithMessage("Description should be set when workLog is not approved");
        }
    }
}