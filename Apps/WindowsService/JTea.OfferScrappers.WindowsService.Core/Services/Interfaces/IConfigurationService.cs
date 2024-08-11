using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services.Interfaces
{
    public interface IConfigurationService
    {
        ConfigurationModel GetConfiguration();

        int GetDbVersion();

        Task<Result<ConfigurationModel>> UpdateConfiguration(ConfigurationModel newConfiguration);
    }
}