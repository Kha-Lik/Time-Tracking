using System;
using FluentValidation;
using TimeTracking.ReportGenerator.Models;

namespace TimeTracking.ReportGenerator.Bl.Impl.Validators
{
    public class ReportConfigParameterValidator : AbstractValidator<ReportConfiguration>
    {
        public ReportConfigParameterValidator()
        {
            RuleFor(x => x.ReportFormatType)
                .IsInEnum();

            RuleFor(x => x.ReportType)
                .IsInEnum();

            RuleFor(x => x.FromDate)
                .DateInValidRange();

            RuleFor(x => x.ToDate)
                .DateInValidRange();

            RuleFor(x => x.ProjectId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty();
        }
    }
}