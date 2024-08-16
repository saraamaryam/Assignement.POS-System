using Microsoft.AspNetCore.Http;
using System.Net;
using static POS.API.Middlewares.CustomExceptions;

namespace POS.Middlewares
{
    public class CustomExceptionHandling
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(new
                {
                    error = ex.Message
                }.ToString());
            }
            catch (NotFoundException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync(new
                {
                    error = ex.Message
                }.ToString());
            }
            catch (UnauthorizedAccessEx ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(new
                {
                    error = ex.Message
                }.ToString());
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(new
                {
                    error = "An unexpected error occurred. Please try again later."
                }.ToString());
            }
        }
    }
}
