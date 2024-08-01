using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public interface IOfferHeadersService
    {
        OfferHeaderModel Create(OfferHeaderModel offerHeader);

        bool Delete(int id);

        void DeleteAll();

        List<OfferHeaderModel> GetAll();

        List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter);

        OfferHeaderModel GetById(int id);

        bool SetEnabled(int id, bool enabled);

        OfferHeaderModel Update(UpdateOfferHeader updateOfferHeader);
    }
}