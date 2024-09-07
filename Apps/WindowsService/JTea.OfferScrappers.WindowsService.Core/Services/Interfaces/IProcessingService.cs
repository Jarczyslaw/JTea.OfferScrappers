using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Core.Services.Interfaces
{
    public interface IProcessingService
    {
        ProcessingState State { get; }

        Task<ScrapResultModel> FetchOffers(FetchOffersArguments arguments);

        Task<ScrapResultModel> TestFetchOffers(ScrapperType scrapperType, PageSourceProviderType pageSourceProviderType);
    }
}