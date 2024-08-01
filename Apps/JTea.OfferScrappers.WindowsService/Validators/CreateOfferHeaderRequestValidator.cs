using FluentValidation;
using JTea.OfferScrappers.WindowsService.Requests;

namespace JTea.OfferScrappers.WindowsService.Validators
{
    public class CreateOfferHeaderRequestValidator : BaseCreateUpdateOfferHeaderRequestValidator<CreateOfferHeaderRequest>
    {
        public CreateOfferHeaderRequestValidator()
        {
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}