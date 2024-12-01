using JTea.OfferScrappers.Logic.Persistence.Abstraction;
using JTea.OfferScrappers.Logic.Persistence.Entities;
using JToolbox.DataAccess.L2DB.Repositories;
using LinqToDB.Data;

namespace JTea.OfferScrappers.Logic.Persistence.Repositories
{
    public class OfferHistoriesRepository : BaseRepository<OfferHistoryEntity>, IOfferHistoriesRepository
    {
        public void DeleteByOfferIds(DataConnection db, List<int> offerIds)
        {
            DeleteBy(db, x => offerIds.Contains(x.OfferId));
        }
    }
}