using JToolbox.Core.Abstraction;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace JTea.OfferScrappers.WindowsService.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILoggerService _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.Error(exception, "An unexpected error occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new ExceptionResponse
            {
                StackTrace = exception.StackTrace,
                Message = exception.Message
            });
        }

        private class ExceptionResponse
        {
            public string Message { get; set; }

            public string StackTrace { get; set; }
        }
    }
}