using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.SQLiteNet;
using JToolbox.DataAccess.SQLiteNet.Repositories;
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

        public List<OfferModel> GetByOfferHeaderId(int offerHeaderId)
        {
            List<OfferEntity> entities = _dataAccessService.Execute(x => GetBy(x, y => y.OfferHeaderId == offerHeaderId));
            return _mapper.Map<List<OfferModel>>(entities);
        }
    }
}