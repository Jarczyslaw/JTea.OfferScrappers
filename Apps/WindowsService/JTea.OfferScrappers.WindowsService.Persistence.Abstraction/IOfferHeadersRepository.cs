using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Persistence.Abstraction
{
    public interface IOfferHeadersRepository
    {
        bool Clear(OfferHeaderModel offerHeader);

        OfferHeaderModel Create(OfferHeaderModel offerHeader);

        bool Delete(int id);

        void DeleteAll();

        List<OfferHeaderModel> GetAll(bool completeData);

        List<OfferHeaderModel> GetByFilter(OfferHeadersFilterModel filter);

        OfferHeaderModel GetById(int id, bool completeData);

        bool SetEnabled(int id, bool enabled);

        bool Update(UpdateOfferHeaderModel updateOfferHeader);
    }
}