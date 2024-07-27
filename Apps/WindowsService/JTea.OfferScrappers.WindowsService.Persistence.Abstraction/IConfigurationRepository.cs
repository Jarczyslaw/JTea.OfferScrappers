using JTea.OfferScrappers.WindowsService.Models;

namespace JTea.OfferScrappers.WindowsService.Persistence.Abstraction
{
    public interface IConfigurationRepository
    {
        Configuration GetConfiguration();

        void UpdateConfiguration(Configuration configuration);
    }
}