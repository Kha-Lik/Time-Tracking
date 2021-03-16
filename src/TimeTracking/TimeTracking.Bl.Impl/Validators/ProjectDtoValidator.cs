using FluentValidation;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Validators
{
    public class ProjectDtoValidator:AbstractValidator<ProjectDto>
    {
        public ProjectDtoValidator()
        {
            RuleFor(x => x.Abbreviation)
                .NotNull()
                .NotEmpty();
            
            RuleFor(x=>x.Name)
                .NotNull()
                .NotEmpty();
            
            RuleFor(x=>x.Description)
                .NotNull()
                .NotEmpty();
            
            RuleFor(e=>e.InitialRisk)
                .NotNull()
                .NotEmpty();
        }
    }
}