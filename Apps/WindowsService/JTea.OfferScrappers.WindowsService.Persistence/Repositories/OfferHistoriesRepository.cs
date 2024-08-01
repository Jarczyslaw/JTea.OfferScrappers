using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.SQLiteNet;
using JToolbox.DataAccess.SQLiteNet.Repositories;
using MapsterMapper;

namespace JTea.OfferScrappers.WindowsService.Persistence.Repositories
{
    public class OfferHistoriesRepository : BaseRepository<OfferHistoryEntity>, IOfferHistoriesRepository
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IMapper _mapper;

        public OfferHistoriesRepository(
            IMapper mapper,
            IDataAccessService dataAccessService)
        {
            _mapper = mapper;
            _dataAccessService = dataAccessService;
        }

        public void Add(OfferHistoryModel model)
        {
            OfferHistoryEntity entity = _mapper.Map<OfferHistoryEntity>(model);
            model.Id = _dataAccessService.Execute(x => Create(x, entity));
        }

        public Dictionary<int, List<OfferHistoryModel>> GetHistories(IEnumerable<int> offerIds)
        {
            List<OfferHistoryEntity> entities
                = _dataAccessService.Execute(x => GetBy(x, y => offerIds.Contains(y.OfferId)));

            List<OfferHistoryModel> models = _mapper.Map<List<OfferHistoryModel>>(entities);
            return models.GroupBy(x => x.OfferId)
                .ToDictionary(x => x.Key, x => x.ToList());
        }
    }
}