using FluentValidation;
using JTea.OfferScrappers.WindowsService.Requests;

namespace JTea.OfferScrappers.WindowsService.Validators
{
    public class UpdateOfferHeaderRequestValidator : BaseCreateUpdateOfferHeaderRequestValidator<UpdateOfferHeaderRequest>
    {
        public UpdateOfferHeaderRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}