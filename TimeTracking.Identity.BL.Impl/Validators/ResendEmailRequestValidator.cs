using FluentValidation;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class ResendEmailRequestValidator : AbstractValidator<ResendEmailRequest>
    {
        public ResendEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();
        }
    }
}