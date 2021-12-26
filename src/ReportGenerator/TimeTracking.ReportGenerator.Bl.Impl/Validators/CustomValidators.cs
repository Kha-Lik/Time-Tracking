using System;
using System.Collections.Generic;
using FluentValidation;

namespace TimeTracking.ReportGenerator.Bl.Impl.Validators
{
    public static class CustomValidators
    {
        private static DateTime MinDateValue = DateTime.Parse("01/01/2001");
        public static IRuleBuilderOptions<T, DateTime> DateInValidRange<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .Must(DataTimeInRange)
                .WithMessage($"Data must be in range from '{MinDateValue.ToShortDateString()}'to '{DateTime.UtcNow.ToShortDateString()}");

        }
        private static bool DataTimeInRange(DateTime value)
        {
            return (value > MinDateValue) && (value < DateTime.UtcNow);
        }
    }
}