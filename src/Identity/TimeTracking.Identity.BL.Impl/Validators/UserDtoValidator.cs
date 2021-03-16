using FluentValidation;
using TimeTracking.Identity.Models.Dtos;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class UserDtoValidator:AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x=>x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(x => x.Id)
                .NotEmpty();
            
            RuleFor(x => x.Phone)
                .NotEmpty()
                .NotNull()
                .Matches(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull();
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .NotNull();
        }
    }
}