using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Api.ExtensionMethods;

namespace Tatisoft.UsersAdmin.Api.Filters
{
    public class ModelValidationActionFilter : IActionFilter
    {
        public readonly ILogger<ModelValidationActionFilter> _logger;

        public ModelValidationActionFilter(ILogger<ModelValidationActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogTrace("{0} -> Validating Model...", context.ActionDescriptor.GetShortMethodName());
            if (!context.ModelState.IsValid)
            {
                string message = this.GetErrorFirstMessage(context.ModelState);
                WarningAnswer answer = new WarningAnswer(message);
                context.Result = new OkObjectResult(answer);
            }
            else
            {
                _logger.LogTrace("{0} -> Model Validated", context.ActionDescriptor.GetShortMethodName());
            }
        }

        private string GetErrorFirstMessage(ModelStateDictionary dictionary)
        {
            return dictionary.SelectMany(m => m.Value.Errors)
                             .Select(m => m.ErrorMessage)
                             .ToList()
                             .First();
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}