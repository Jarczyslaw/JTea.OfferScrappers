using JTea.OfferScrappers.WindowsService.Models.Exceptions;
using JToolbox.Core.Abstraction;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace JTea.OfferScrappers.WindowsService.ErrorHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILoggerService _loggerService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GlobalExceptionHandler(
            IWebHostEnvironment webHostEnvironment,
            ILoggerService loggerService)
        {
            _webHostEnvironment = webHostEnvironment;
            _loggerService = loggerService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";

            if (exception.GetType().IsAssignableTo(typeof(DefinedException)))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _loggerService.Error(exception, "Unhandled exception");
            }

            var error = new RequestError()
            {
                ExceptionType = exception.GetType().Name,
                Message = exception.Message,
                StackTrace = _webHostEnvironment.EnvironmentName == Environments.Development
                    ? exception.StackTrace
                    : string.Empty
            };

            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            return true;
        }
    }
}