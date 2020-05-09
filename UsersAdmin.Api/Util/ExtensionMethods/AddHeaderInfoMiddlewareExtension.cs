using UsersAdmin.Api.Util.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace UsersAdmin.Api.Util.ExtensionMethods
{
    public static class AddHeaderInfoMiddlewareExtension
    {
        public static IApplicationBuilder UseAddHeaderInfo(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<AddHeaderInfoMiddleware>();
        }
    }
}