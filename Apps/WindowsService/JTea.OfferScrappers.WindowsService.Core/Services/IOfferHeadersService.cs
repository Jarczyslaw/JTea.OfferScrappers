using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public interface IOfferHeadersService
    {
        Result<OfferHeaderModel> Create(OfferHeaderModel offerHeader);

        Result<bool> Delete(int id);

        void DeleteAll();

        List<OfferHeaderModel> GetAll();

        List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter);

        Result<OfferHeaderModel> GetById(int id);

        Result<bool> SetEnabled(int id, bool enabled);

        Result<OfferHeaderModel> Update(UpdateOfferHeader updateOfferHeader);
    }
}