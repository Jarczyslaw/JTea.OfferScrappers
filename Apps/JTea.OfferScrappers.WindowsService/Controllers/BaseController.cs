using JTea.OfferScrappers.WindowsService.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected void CheckModel()
        {
            if (!ModelState.IsValid)
            {
                string validationMessage = string.Join(". ", ModelState.Values
                    .SelectMany(x => x.Errors.Select(x => x.ErrorMessage)));

                throw new ModelValidationException(validationMessage);
            }
        }
    }
}