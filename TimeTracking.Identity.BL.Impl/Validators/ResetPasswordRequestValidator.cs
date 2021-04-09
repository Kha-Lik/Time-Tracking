using FluentValidation;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty();
        }
    }
}