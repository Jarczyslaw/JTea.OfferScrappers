using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace JTea.OfferScrappers.WindowsService
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GlobalExceptionHandler(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var error = new
            {
                Type = exception.GetType().Name,
                exception.Message,
                StackTrace = _webHostEnvironment.EnvironmentName == Environments.Development
                    ? exception.StackTrace
                    : string.Empty
            };

            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            return true;
        }
    }
}