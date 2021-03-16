using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TimeTracking.Common.Wrapper;

namespace TimeTracking.Common.Helpers
{
     public class ModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result =
                    new JsonResult(new ApiResponse(
                        new ApiError(ErrorCode.ValidationError, GetModelErrors(context.ModelState)),statusCode:400));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //intentionally left empty
        }

        private IEnumerable<ValidationFailure> GetModelErrors(ModelStateDictionary modelState)
        {
            return (from modelStateInfo in modelState 
                    from modelError in modelStateInfo.Value?.Errors 
                    select new ValidationFailure(modelStateInfo.Key, modelError.ErrorMessage)
                    ).ToList();
        }
    }
}