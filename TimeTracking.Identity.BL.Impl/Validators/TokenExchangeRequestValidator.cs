using FluentValidation;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class TokenExchangeRequestValidator : AbstractValidator<TokenExchangeRequest>
    {
        public TokenExchangeRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
            
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}