using JTea.OfferScrappers.Logic.Core.Services.Interfaces;
using JTea.OfferScrappers.Logic.Models.Domain;
using JTea.OfferScrappers.Logic.Models.Exceptions;
using JTea.OfferScrappers.Logic.Persistence.Abstraction;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.Logic.Core.Services
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

        public List<OfferHeaderModel> GetByFilter(OfferHeadersFilterModel filter) => _offerHeadersRepository.GetByFilter(filter);

        public Result<OfferHeaderModel> GetById(int id)
        {
            OfferHeaderModel result = _offerHeadersRepository.GetById(id, completeData: true);
            if (result == null) { return Result<OfferHeaderModel>.AsError(new OfferHeaderNotFoundException(id)); }

            return new(result);
        }
    }
}