using System.Net;
using System.Text.Json;

namespace ArabRiver.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware>
            _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;

            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    ex.Message);

                await HandleExceptionAsync(
                    context);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context)
        {
            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                Success = false,

                Message =
                    "Something went wrong. Please try again later."
            };

            var jsonResponse =
                JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(
                jsonResponse);
        }
    }
}
