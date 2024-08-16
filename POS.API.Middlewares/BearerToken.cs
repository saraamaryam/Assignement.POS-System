using Microsoft.AspNetCore.Http;
using POS.API.Services;
using System.Security.Claims;

public class BearerToken
{
    private readonly RequestDelegate _next;
    private readonly TokenServices _tokenService;
    
    public BearerToken(RequestDelegate next, TokenServices tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromToken(token);
                if (principal != null)
                {
                    context.User = new ClaimsPrincipal(principal);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await _next(context);
    }
}
