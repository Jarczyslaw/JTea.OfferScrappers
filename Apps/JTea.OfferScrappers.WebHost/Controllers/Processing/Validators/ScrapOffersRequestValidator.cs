using FluentValidation;
using JTea.OfferScrappers.WebHost.Controllers.Processing.Requests;

namespace JTea.OfferScrappers.WebHost.Controllers.Processing.Validators
{
    public class ScrapOffersRequestValidator : AbstractValidator<ScrapOffersRequest>
    {
        public ScrapOffersRequestValidator()
        {
            RuleFor(x => x.OfferType).IsInEnum();
            RuleFor(x => x.PageSourceProviderType).IsInEnum();
            RuleFor(x => x.OfferUrl).NotEmpty();
        }
    }
}