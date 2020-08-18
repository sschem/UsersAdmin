using Microsoft.AspNetCore.Builder;
using Tatisoft.UsersAdmin.Api.Middlewares;

namespace Tatisoft.UsersAdmin.Api.ExtensionMethods
{
    public static class AddHeaderInfoMiddlewareExtension
    {
        public static IApplicationBuilder UseAddHeaderInfo(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<AddHeaderInfoMiddleware>();
        }
    }
}