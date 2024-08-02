using FluentValidation;
using JTea.OfferScrappers.WindowsService.Controllers.Configuration.Requests;

namespace JTea.OfferScrappers.WindowsService.Controllers.Configuration.Validators
{
    public class UpdateConfigurationRequestValidator : AbstractValidator<UpdateConfigurationRequest>
    {
        public UpdateConfigurationRequestValidator()
        {
            RuleFor(x => x.CronExpression).NotEmpty();
        }
    }
}