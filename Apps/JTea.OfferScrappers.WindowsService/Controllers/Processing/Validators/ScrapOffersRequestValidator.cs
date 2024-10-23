using FluentValidation;
using JTea.OfferScrappers.WindowsService.Controllers.Processing.Requests;

namespace JTea.OfferScrappers.WindowsService.Controllers.Processing.Validators
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