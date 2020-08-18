using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Tatisoft.UsersAdmin.Api.ExtensionMethods;

namespace Tatisoft.UsersAdmin.Api.Filters
{
    public class InitEndLogActionFilter : IActionFilter
    {
        public readonly ILogger<InitEndLogActionFilter> _logger;

        public InitEndLogActionFilter(ILogger<InitEndLogActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //TODO: add parameters to log
            _logger.LogTrace("{0} -> Init", context.ActionDescriptor.GetShortMethodName());
            // foreach (var argument in context.ActionArguments)
            // {
            //     object parameterValue = argument.Value;
            //     string parameterName = argument.Key;
            // }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{0} -> End", context.ActionDescriptor.GetShortMethodName());
        }
    }
}