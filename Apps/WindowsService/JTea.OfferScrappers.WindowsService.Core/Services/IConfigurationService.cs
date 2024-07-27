using JTea.OfferScrappers.WindowsService.Models;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public interface IConfigurationService
    {
        Configuration GetConfiguration();

        Task StartNow();

        Task UpdateConfiguration(Configuration newConfiguration);
    }
}