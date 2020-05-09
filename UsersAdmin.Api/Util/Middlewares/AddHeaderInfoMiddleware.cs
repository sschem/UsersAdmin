using System.Diagnostics;
using System.Threading.Tasks;
using UsersAdmin.Api.Util.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UsersAdmin.Api.Util.Middlewares
{
    public class AddHeaderInfoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly SystemInfoConfig _settings;
        // ILoggerFactory _loggerFactory;

        public AddHeaderInfoMiddleware(RequestDelegate next, IOptions<SystemInfoConfig> options, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<AddHeaderInfoMiddleware>();
            _settings = options.Value;
            // _loggerFactory = loggerFactory;            
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // var otherLogger = _loggerFactory.CreateLogger(typeof(Controllers.SystemController));
            // otherLogger.LogInformation("I am another logger!!!!!!!!!!!!!!!!!!!!!!");           
            httpContext.Response.OnStarting(() =>
            {
                _logger.LogTrace("Adding Info headers...");

                httpContext.Response.Headers.Add("SystemName", new string[] { _settings.SystemName });
                httpContext.Response.Headers.Add("SystemComment", new string[] { _settings.SystemComment });
                httpContext.Response.Headers.Add("TraceId", new string[] { Activity.Current?.Id ?? httpContext.TraceIdentifier ?? "-" });

                return Task.FromResult(0);
            });

            await _next.Invoke(httpContext);
        }
    }
}