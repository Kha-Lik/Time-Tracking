using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace TimeTracking.Templates.Services
{
    public interface IRazorViewFinder
    {
        bool FindView(string viewName);
    }

    public class RazorViewFinder : IRazorViewFinder
    {
        private IRazorViewEngine _viewEngine;
        private readonly ILogger<RazorViewFinder> _logger;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;
        public RazorViewFinder(ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IRazorViewEngine viewEngine,
            ILogger<RazorViewFinder> logger)
        {
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _viewEngine = viewEngine;
            _logger = logger;
        }

        public bool FindView(string viewName)
        {
            var actionContext = GetActionContext();
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return true;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return true;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;
            _logger.LogError(errorMessage);
            return false;
        }
        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = _serviceProvider;
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}