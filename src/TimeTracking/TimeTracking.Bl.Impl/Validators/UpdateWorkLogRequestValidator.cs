using FluentValidation;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Impl.Validators
{
    public class UpdateWorkLogRequestValidator:AbstractValidator<WorkLogUpdateRequest>
    {
        public UpdateWorkLogRequestValidator()
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
            
            RuleFor(x => x.WorkLogId)
                .NotEmpty()
                .NotNull();
        }
    }
}