using FluentValidation;
using JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Requests;

namespace JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Validators
{
    public class BaseCreateUpdateOfferHeaderRequestValidator<T> : AbstractValidator<T>
        where T : BaseCreateUpdateOfferHeaderRequest
    {
        public BaseCreateUpdateOfferHeaderRequestValidator()
        {
            RuleFor(x => x.OfferUrl).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
        }
    }
}