using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using UsersAdmin.Api.Util.ExtensionMethods;

namespace UsersAdmin.Api.Util.Filters
{
    public class StringLogResultFilter : IResultFilter
    {
        private readonly ILogger<StringLogResultFilter> _logger;
        private JsonSerializerOptions _serializeOptions;

        public StringLogResultFilter(ILogger<StringLogResultFilter> logger)
        {
            _logger = logger;
            _serializeOptions = new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
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