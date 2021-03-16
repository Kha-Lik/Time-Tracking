using FluentValidation;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class RevokeTokenRequestValidator:AbstractValidator<RevokeTokenRequest>
    {
        public RevokeTokenRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotNull()
                .NotEmpty();
        }
    }
}