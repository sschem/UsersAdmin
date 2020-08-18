using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tatisoft.UsersAdmin.Api.Config;

namespace Tatisoft.UsersAdmin.Api.Middlewares
{
    public class AddHeaderInfoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly SystemInfoConfig _settings;

        public AddHeaderInfoMiddleware(RequestDelegate next, IOptions<SystemInfoConfig> options, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<AddHeaderInfoMiddleware>();
            _settings = options.Value;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.OnStarting(() =>
            {
                _logger.LogTrace("Adding Info headers...");

                httpContext.Response.Headers.Add("SystemName", new string[] { _settings.SystemName });
                httpContext.Response.Headers.Add("SystemComment", new string[] { _settings.SystemComment });
                httpContext.Response.Headers.Add("TraceId", new string[] { Activity.Current?.Id ?? httpContext.TraceIdentifier ?? "-" });
                httpContext.Response.Headers.Add("Version", Assembly.GetEntryAssembly().GetName().Version.ToString());

                return Task.FromResult(0);
            });

            await _next.Invoke(httpContext);
        }
    }
}