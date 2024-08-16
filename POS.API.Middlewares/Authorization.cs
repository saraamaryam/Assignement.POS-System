using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace POS.API.Middlewares
{
    public class Authorization
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Authorization> _log;
        private readonly string _requiredRole;

        public Authorization(RequestDelegate next, ILogger<Authorization> logger, string requiredRole)
        {
            _next = next;
            _log = logger;
            _requiredRole = requiredRole;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            if (path.StartsWith("/api/Authentication", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User is not authenticated");
                return;
            }
            
            if (path.StartsWith("/api/Transaction", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
            _log.LogInformation("User Role: {Role}", userRole);

            if (userRole != _requiredRole)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("User does not have the required role");
                return;
            }

            await _next(context);
        }
    }

}
