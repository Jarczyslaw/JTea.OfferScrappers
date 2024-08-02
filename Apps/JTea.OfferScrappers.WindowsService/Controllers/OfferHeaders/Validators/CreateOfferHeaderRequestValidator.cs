using FluentValidation;
using JTea.OfferScrappers.WindowsService.Controllers.OfferHeaders.Requests;

namespace JTea.OfferScrappers.WindowsService.Controllers.OfferHeaders.Validators
{
    public class CreateOfferHeaderRequestValidator : BaseCreateUpdateOfferHeaderRequestValidator<CreateOfferHeaderRequest>
    {
        public CreateOfferHeaderRequestValidator()
        {
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}