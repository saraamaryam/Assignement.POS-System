using Microsoft.AspNetCore.Http;
using POS.API.Services.UserServices;
using System.Security.Claims;
using System.Text;

namespace POS.API.Middlewares
{
    public class BasicAuth
    {
        private readonly RequestDelegate _next;
        private readonly string trainingKey = "TrainingKey";

        public BasicAuth(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserService userService)
        {
            if (!context.Request.Headers.TryGetValue("AuthKey", out var authKey) || authKey != trainingKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid AuthKey");
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Basic ".Length).Trim();

                var credentialBytes = Convert.FromBase64String(token);

                var credentialsString = Encoding.UTF8.GetString(credentialBytes);

                var credentials = credentialsString.Split(':');
              
                if (credentials.Length > 0)
                {
                    Console.WriteLine("Credentials[0]: {0}", credentials[0]);
                }
                if (credentials.Length > 1)
                {
                    Console.WriteLine("Credentials[1]: {0}", credentials[1]);
                }

                if (credentials.Length == 2)
                {
                    var username = credentials[0];
                    var password = credentials[1];
                   
                    var user = userService.Login(username,password);
                    if (user != null)
                    {
                        var authenticatedUser = user.Result;
                        if (authenticatedUser != null)
                        {
                            Console.WriteLine(authenticatedUser.role);
                            var claims = new[]
                            {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, authenticatedUser.role.ToString()),
                            new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Id.ToString())
                        };

                            var identity = new ClaimsIdentity(claims, "Basic");
                            context.User = new ClaimsPrincipal(identity);

                            await _next(context);
                            return;
                        }
                    }
                }
            }
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid credentials");


            
        }
    }
}
