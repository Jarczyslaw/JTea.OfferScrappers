using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services.Interfaces
{
    public interface IReportsService
    {
        List<OfferHeaderModel> GetAll();

        Result<OfferHeaderModel> GetById(int id);
    }
}