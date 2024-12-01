using FluentValidation;
using JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Requests;

namespace JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Validators
{
    public class UpdateOfferHeaderRequestValidator : BaseCreateUpdateOfferHeaderRequestValidator<UpdateOfferHeaderRequest>
    {
        public UpdateOfferHeaderRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}