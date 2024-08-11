using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Persistence.Abstraction
{
    public interface IConfigurationRepository
    {
        ConfigurationModel GetConfiguration();

        int GetDbVersion();

        void UpdateConfiguration(ConfigurationModel configuration);
    }
}