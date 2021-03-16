using FluentValidation;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Impl.Validators
{
    public class UpdateWorkLogStatusRequestValidator:AbstractValidator<UpdateWorkLogStatusRequest>
    {
        public UpdateWorkLogStatusRequestValidator()
        {
            RuleFor(s => s.IsApproved)
                .NotNull();

            RuleFor(d => d.WorkLogId)
                .NotEmpty();
        }
    }
}