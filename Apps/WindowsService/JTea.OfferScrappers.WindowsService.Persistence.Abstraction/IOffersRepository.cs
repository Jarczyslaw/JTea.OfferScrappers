using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Persistence.Abstraction
{
    public interface IOffersRepository
    {
        List<OfferModel> GetByOfferHeaderId(int offerHeaderId);
    }
}