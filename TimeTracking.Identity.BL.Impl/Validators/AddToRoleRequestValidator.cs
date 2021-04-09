using FluentValidation;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class AddToRoleRequestValidator : AbstractValidator<AddToRoleRequest>
    {
        public AddToRoleRequestValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}