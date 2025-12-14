using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SimpleApi.src.Models;

namespace SimpleApi.Middleware
{
    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;

    public class ActiveUserMiddleware
    {
        private readonly RequestDelegate _next;
            private readonly IServiceProvider _serviceProvider;

        public ActiveUserMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only check for /api/students endpoints
            if (context.Request.Path.StartsWithSegments("/api/students"))
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userId != null)
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                            var user = await userManager.FindByIdAsync(userId);
                            if (user != null && user.IsActive == false)
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync("{ \"error\": \"Your account is inactive. Please contact an administrator.\" }");
                                return;
                            }
                        }
                    }
                }
            }
            await _next(context);
        }
    }
}
