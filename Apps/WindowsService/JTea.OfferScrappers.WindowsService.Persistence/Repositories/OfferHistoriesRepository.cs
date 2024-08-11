using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.L2DB;
using JToolbox.DataAccess.L2DB.Repositories;
using LinqToDB.Data;
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

        public void DeleteByOfferIds(DataConnection db, List<int> offerIds)
        {
            DeleteBy(db, x => offerIds.Contains(x.OfferId));
        }
    }
}