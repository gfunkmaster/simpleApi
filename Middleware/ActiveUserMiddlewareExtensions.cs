using Microsoft.AspNetCore.Builder;

namespace SimpleApi.Middleware
{
    public static class ActiveUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseActiveUserCheck(this IApplicationBuilder builder)
            => builder.UseMiddleware<ActiveUserMiddleware>();
    }
}
