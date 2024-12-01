using FluentValidation;
using JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Requests;

namespace JTea.OfferScrappers.WebHost.Controllers.OfferHeaders.Validators
{
    public class CreateOfferHeaderRequestValidator : BaseCreateUpdateOfferHeaderRequestValidator<CreateOfferHeaderRequest>
    {
        public CreateOfferHeaderRequestValidator()
        {
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}