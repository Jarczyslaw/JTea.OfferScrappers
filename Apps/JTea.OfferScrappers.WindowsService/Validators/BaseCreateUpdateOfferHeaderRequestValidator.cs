using FluentValidation;
using JTea.OfferScrappers.WindowsService.Requests;

namespace JTea.OfferScrappers.WindowsService.Validators
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