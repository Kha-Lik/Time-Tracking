using System.Diagnostics;
using FluentValidation;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Impl.Validators
{
    public class ActivitiesRequestValidator:AbstractValidator<ActivitiesRequest>
    {
        public ActivitiesRequestValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .NotNull();
            
            RuleFor(r=>r.UserId)
                .NotEmpty()
                .NotNull();
                
        }
    }
}