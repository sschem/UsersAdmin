using Microsoft.AspNetCore.Builder;
using UsersAdmin.Api.Middlewares;

namespace UsersAdmin.Api.ExtensionMethods
{
    public static class AddHeaderInfoMiddlewareExtension
    {
        public static IApplicationBuilder UseAddHeaderInfo(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<AddHeaderInfoMiddleware>();
        }
    }
}