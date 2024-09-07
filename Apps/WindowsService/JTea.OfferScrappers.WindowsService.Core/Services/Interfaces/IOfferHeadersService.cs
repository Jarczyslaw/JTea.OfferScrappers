using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services.Interfaces
{
    public interface IOfferHeadersService
    {
        Result Clear(int id);

        Result<OfferHeaderModel> Create(OfferHeaderModel offerHeader);

        Result Delete(int id);

        Result DeleteAll();

        List<OfferHeaderModel> GetAll();

        Result<OfferHeaderModel> GetById(int id);

        Result SetEnabled(int id, bool enabled);

        Result<OfferHeaderModel> Update(UpdateOfferHeader updateOfferHeader);
    }
}