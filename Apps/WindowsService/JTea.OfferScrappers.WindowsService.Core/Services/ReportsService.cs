using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IOfferHeadersRepository _offerHeadersRepository;
        private readonly IOfferHistoriesRepository _offerHistoriesRepository;
        private readonly IOffersRepository _offersRepository;

        public ReportsService(
            IOfferHeadersRepository offerHeadersRepository,
            IOffersRepository offersRepository,
            IOfferHistoriesRepository offerHistoriesRepository)
        {
            _offerHeadersRepository = offerHeadersRepository;
            _offersRepository = offersRepository;
            _offerHistoriesRepository = offerHistoriesRepository;
        }
    }
}