using LinqToDB.Data;

namespace JTea.OfferScrappers.WindowsService.Persistence.Abstraction
{
    public interface IOffersRepository
    {
        int DeleteMany(DataConnection db, List<int> ids);

        List<int> GetOfferIdsByOfferHeaderId(DataConnection db, int offerHeaderId);
    }
}