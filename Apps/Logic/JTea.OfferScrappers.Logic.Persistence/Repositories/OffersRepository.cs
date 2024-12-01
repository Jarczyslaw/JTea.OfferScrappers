using JTea.OfferScrappers.Logic.Persistence.Abstraction;
using JTea.OfferScrappers.Logic.Persistence.Entities;
using JToolbox.DataAccess.L2DB.Repositories;
using LinqToDB;
using LinqToDB.Data;

namespace JTea.OfferScrappers.Logic.Persistence.Repositories
{
    public class OffersRepository : BaseRepository<OfferEntity>, IOffersRepository
    {
        public List<int> GetOfferIdsByOfferHeaderId(DataConnection db, int offerHeaderId)
        {
            return db.GetTable<OfferEntity>()
                .Where(x => x.OfferHeaderId == offerHeaderId)
                .Select(x => x.Id)
                .ToList();
        }
    }
}