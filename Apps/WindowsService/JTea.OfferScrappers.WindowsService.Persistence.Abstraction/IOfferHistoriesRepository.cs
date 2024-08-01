using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Persistence.Abstraction
{
    public interface IOfferHistoriesRepository
    {
        void Add(OfferHistoryModel model);

        Dictionary<int, List<OfferHistoryModel>> GetHistories(IEnumerable<int> offerIds);
    }
}