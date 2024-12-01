using JTea.OfferScrappers.Logic.Models.Domain;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.Logic.Core.Services.Interfaces
{
    public interface IProcessingService
    {
        bool IsRunning { get; }

        Task<Result> ProcessAllOfferHeaders(bool waitIfCurrentlyProcessing);

        Task<ScrapResultModel> ScrapOffers(ScrapOffersArgumentsModel arguments);

        Task<ScrapResultModel> ScrapTestOffers(ScrapperType scrapperType, PageSourceProviderType pageSourceProviderType);
    }
}