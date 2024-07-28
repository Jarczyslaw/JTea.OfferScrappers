using FluentValidation;
using JTea.OfferScrappers.WindowsService.Requests;

namespace JTea.OfferScrappers.WindowsService.Validators
{
    public class UpdateConfigurationRequestValidator : AbstractValidator<UpdateConfigurationRequest>
    {
        public UpdateConfigurationRequestValidator()
        {
            RuleFor(x => x.CronExpression).NotEmpty();
        }
    }
}