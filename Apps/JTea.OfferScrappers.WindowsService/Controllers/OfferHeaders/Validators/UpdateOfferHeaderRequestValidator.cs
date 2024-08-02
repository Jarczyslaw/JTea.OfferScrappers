using FluentValidation;
using JTea.OfferScrappers.WindowsService.Controllers.OfferHeaders.Requests;

namespace JTea.OfferScrappers.WindowsService.Controllers.OfferHeaders.Validators
{
    public class UpdateOfferHeaderRequestValidator : BaseCreateUpdateOfferHeaderRequestValidator<UpdateOfferHeaderRequest>
    {
        public UpdateOfferHeaderRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}