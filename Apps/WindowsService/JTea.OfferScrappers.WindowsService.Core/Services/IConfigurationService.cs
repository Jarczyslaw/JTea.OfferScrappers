using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public interface IConfigurationService
    {
        ConfigurationModel GetConfiguration();

        Task StartNow();

        Task UpdateConfiguration(ConfigurationModel newConfiguration);
    }
}