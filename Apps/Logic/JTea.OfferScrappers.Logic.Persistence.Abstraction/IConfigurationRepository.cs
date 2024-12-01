using JTea.OfferScrappers.Logic.Models.Domain;

namespace JTea.OfferScrappers.Logic.Persistence.Abstraction
{
    public interface IConfigurationRepository
    {
        ConfigurationModel GetConfiguration();

        int GetDbVersion();

        void UpdateConfiguration(ConfigurationModel configuration);
    }
}