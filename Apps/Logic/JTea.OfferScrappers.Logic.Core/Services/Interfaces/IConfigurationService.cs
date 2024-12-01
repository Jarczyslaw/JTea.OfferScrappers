using JTea.OfferScrappers.Logic.Models.Domain;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.Logic.Core.Services.Interfaces
{
    public interface IConfigurationService
    {
        ConfigurationModel GetConfiguration();

        int GetDbVersion();

        Task<Result<ConfigurationModel>> UpdateConfiguration(ConfigurationModel newConfiguration);
    }
}