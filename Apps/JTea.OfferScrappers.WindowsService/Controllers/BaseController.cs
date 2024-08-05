using JTea.OfferScrappers.WindowsService.ErrorHandling;
using JTea.OfferScrappers.WindowsService.Exceptions;
using JToolbox.Core.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        [HttpGet("ping")]
        public ActionResult<bool> Ping() => Ok(true);

        protected void CheckModel()
        {
            if (!ModelState.IsValid)
            {
                string validationMessage = string.Join(". ", ModelState.Values
                    .SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));

                throw new ModelValidationException(validationMessage);
            }
        }

        protected ActionResult CreateActionResult<T>(Result<T> result, Func<T, object> converter = null)
        {
            if (result.IsError)
            {
                return CreateBadRequest(result);
            }

            object response = converter == null
                ? result.Value
                : converter(result.Value);

            return Ok(response);
        }

        protected ActionResult CreateActionResult(Result result)
        {
            return result.IsError
                ? CreateBadRequest(result)
                : Ok();
        }

        private ActionResult CreateBadRequest(Result result)
        {
            return BadRequest(new RequestError
            {
                Message = result.Error.Content,
                ExceptionType = result.Error.Exception?.GetType().Name
            });
        }
    }
}