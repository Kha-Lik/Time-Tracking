using FluentValidation;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class RegistrationRequestValidator:AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();
            
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8);
            
            RuleFor(x => x.Username)
                .NotEmpty()
                .NotNull();
            
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.LastName)
                .NotEmpty()
                .NotNull();

        }
    }
}