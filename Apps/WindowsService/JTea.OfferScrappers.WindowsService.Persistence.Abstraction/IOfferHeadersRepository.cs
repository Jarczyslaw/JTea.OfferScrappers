using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Persistence.Abstraction
{
    public interface IOfferHeadersRepository
    {
        bool Clear(int offerHeaderId);

        OfferHeaderModel Create(OfferHeaderModel offerHeader);

        bool Delete(int id);

        void DeleteAll();

        List<OfferHeaderModel> GetAll(bool completeData);

        List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter);

        OfferHeaderModel GetById(int id, bool completeData);

        bool SetEnabled(int id, bool enabled);

        bool Update(UpdateOfferHeader updateOfferHeader);
    }
}