using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using UsersAdmin.Api.Util.ExtensionMethods;
using System.Text.Encodings.Web;

namespace UsersAdmin.Api.Util.Filters
{
    public class JsonLogResultFilter : IResultFilter
    {
        private readonly ILogger<JsonLogResultFilter> _logger;
        private JsonSerializerOptions _serializeOptions;

        public JsonLogResultFilter(ILogger<JsonLogResultFilter> logger)
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
                    , JsonSerializer.Serialize(result.Value, _serializeOptions));
            }
            else
            {
                _logger.LogInformation("{0} -> Result: No response available!", context.ActionDescriptor.GetShortMethodName());
            }
        }

        public void OnResultExecuting(ResultExecutingContext context) { }
    }
}