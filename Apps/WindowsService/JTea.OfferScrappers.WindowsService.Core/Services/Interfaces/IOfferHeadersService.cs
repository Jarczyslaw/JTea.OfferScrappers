using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services.Interfaces
{
    public interface IOfferHeadersService
    {
        Result<OfferHeaderModel> Create(OfferHeaderModel offerHeader);

        Result Delete(int id);

        Result DeleteAll();

        List<OfferHeaderModel> GetAll();

        List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter);

        Result<OfferHeaderModel> GetById(int id);

        Result SetEnabled(int id, bool enabled);

        Result<OfferHeaderModel> Update(UpdateOfferHeader updateOfferHeader);
    }
}