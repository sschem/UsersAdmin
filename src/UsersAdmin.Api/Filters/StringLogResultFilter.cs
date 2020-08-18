using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Tatisoft.UsersAdmin.Api.ExtensionMethods;

namespace Tatisoft.UsersAdmin.Api.Filters
{
    public class StringLogResultFilter : IResultFilter
    {
        private readonly ILogger<StringLogResultFilter> _logger;

        public StringLogResultFilter(ILogger<StringLogResultFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is Microsoft.AspNetCore.Mvc.ObjectResult result)
            {
                _logger.LogInformation("{0} -> Result: {1}"
                    , context.ActionDescriptor.GetShortMethodName()
                    , result.Value.ToString());
            }
            else
            {
                _logger.LogInformation("{0} -> No result available!", context.ActionDescriptor.GetShortMethodName());
            }
        }

        public void OnResultExecuting(ResultExecutingContext context) { }
    }
}