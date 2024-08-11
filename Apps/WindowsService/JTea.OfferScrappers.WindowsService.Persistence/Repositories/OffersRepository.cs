using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.L2DB;
using JToolbox.DataAccess.L2DB.Repositories;
using LinqToDB;
using LinqToDB.Data;
using MapsterMapper;

namespace JTea.OfferScrappers.WindowsService.Persistence.Repositories
{
    public class OffersRepository : BaseRepository<OfferEntity>, IOffersRepository
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IMapper _mapper;

        public OffersRepository(
            IMapper mapper,
            IDataAccessService dataAccessService)
        {
            _mapper = mapper;
            _dataAccessService = dataAccessService;
        }

        public List<int> GetOfferIdsByOfferHeaderId(DataConnection db, int offerHeaderId)
        {
            return db.GetTable<OfferEntity>()
                .Where(x => x.OfferHeaderId == offerHeaderId)
                .Select(x => x.Id)
                .ToList();
        }
    }
}