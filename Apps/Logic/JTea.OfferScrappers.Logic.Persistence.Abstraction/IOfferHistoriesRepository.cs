using LinqToDB.Data;

namespace JTea.OfferScrappers.Logic.Persistence.Abstraction
{
    public interface IOfferHistoriesRepository
    {
        void DeleteByOfferIds(DataConnection db, List<int> offerIds);
    }
}