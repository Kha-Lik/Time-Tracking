using FluentValidation;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class EmailConfirmationRequestValidator:AbstractValidator<EmailConfirmationRequest>
    {
        public EmailConfirmationRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty();
            
            RuleFor(x=>x.Code)
                .NotNull()
                .NotEmpty();
        }
    }
}