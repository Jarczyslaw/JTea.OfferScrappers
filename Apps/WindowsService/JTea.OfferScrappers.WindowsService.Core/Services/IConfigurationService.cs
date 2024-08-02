using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public interface IConfigurationService
    {
        ConfigurationModel GetConfiguration();

        Task StartNow();

        Task<Result<ConfigurationModel>> UpdateConfiguration(ConfigurationModel newConfiguration);
    }
}