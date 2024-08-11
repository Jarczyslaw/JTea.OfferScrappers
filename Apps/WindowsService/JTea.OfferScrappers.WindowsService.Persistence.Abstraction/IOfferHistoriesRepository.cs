using LinqToDB.Data;

namespace JTea.OfferScrappers.WindowsService.Persistence.Abstraction
{
    public interface IOfferHistoriesRepository
    {
        void DeleteByOfferIds(DataConnection db, List<int> offerIds);
    }
}