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

        public Result<OfferHeaderModel> GetFullOfferHeader(int id)
        {
            OfferHeaderModel offerHeader = _offerHeadersRepository.GetById(id);
            if (offerHeader == null) { return Result<OfferHeaderModel>.AsError(new OfferHeaderNotFoundException(id)); }

            List<OfferModel> offers = _offersRepository.GetByOfferHeaderId(offerHeader.Id);

            if (offers.Count > 0)
            {
                List<int> offerIds = offers.ConvertAll(x => x.Id);
                Dictionary<int, List<OfferHistoryModel>> histories = _offerHistoriesRepository.GetHistories(offerIds);

                offers.ForEach(x => x.Histories = histories.GetValueOrDefault(x.Id));
            }

            return new Result<OfferHeaderModel>(offerHeader);
        }
    }
}