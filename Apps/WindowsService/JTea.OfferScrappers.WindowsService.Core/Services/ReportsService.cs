using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Models.Exceptions;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IOfferHeadersRepository _offerHeadersRepository;

        public ReportsService(
            IOfferHeadersRepository offerHeadersRepository)
        {
            _offerHeadersRepository = offerHeadersRepository;
        }

        public List<OfferHeaderModel> GetAll() => _offerHeadersRepository.GetAll(completeData: true);

        public Result<OfferHeaderModel> GetById(int id)
        {
            OfferHeaderModel result = _offerHeadersRepository.GetById(id, completeData: true);
            if (result == null) { return Result<OfferHeaderModel>.AsError(new OfferHeaderNotFoundException(id)); }

            return new(result);
        }
    }
}