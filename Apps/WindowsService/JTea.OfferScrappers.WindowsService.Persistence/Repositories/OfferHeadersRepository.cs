using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.L2DB;
using JToolbox.DataAccess.L2DB.Repositories;
using LinqToDB.Data;
using MapsterMapper;
using System.Linq.Expressions;

namespace JTea.OfferScrappers.WindowsService.Persistence.Repositories
{
    public class OfferHeadersRepository : BaseRepository<OfferHeaderEntity>, IOfferHeadersRepository
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IMapper _mapper;
        private readonly IOfferHistoriesRepository _offerHistoriesRepository;
        private readonly IOffersRepository _offersRepository;

        public OfferHeadersRepository(
            IMapper mapper,
            IDataAccessService dataAccessService,
            IOffersRepository offersRepository,
            IOfferHistoriesRepository offerHistoriesRepository)
        {
            _mapper = mapper;
            _dataAccessService = dataAccessService;
            _offersRepository = offersRepository;
            _offerHistoriesRepository = offerHistoriesRepository;
        }

        public void Clear(int offerHeaderId)
        {
            _dataAccessService.ExecuteTransaction(x => DeleteOffersAndHistories(x, offerHeaderId));
        }

        public OfferHeaderModel Create(OfferHeaderModel offerHeader)
        {
            OfferHeaderEntity entity = _mapper.Map<OfferHeaderEntity>(offerHeader);
            offerHeader.Id = _dataAccessService.Execute(x => Create(x, entity));
            return offerHeader;
        }

        public bool Delete(int offerHeaderId)
        {
            return _dataAccessService.ExecuteTransaction(x =>
            {
                DeleteOffersAndHistories(x, offerHeaderId);
                return Delete(x, offerHeaderId);
            });
        }

        public void DeleteAll()
        {
            _dataAccessService.ExecuteTransaction(x =>
            {
                x.Execute($"DELETE FROM {L2DbExtensions.GetTableName(typeof(OfferHistoryEntity))}");
                x.Execute($"DELETE FROM {L2DbExtensions.GetTableName(typeof(OfferEntity))}");
                x.Execute($"DELETE FROM {L2DbExtensions.GetTableName(typeof(OfferHeaderEntity))}");
            });
        }

        public List<OfferHeaderModel> GetAll()
        {
            List<OfferHeaderEntity> entities = _dataAccessService.Execute(GetAll);
            return _mapper.Map<List<OfferHeaderModel>>(entities);
        }

        public List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter)
        {
            List<Expression<Func<OfferHeaderEntity, bool>>> expressions = [];

            if (filter.Type != null) { expressions.Add(x => x.Type == filter.Type); }

            if (filter.Id != null) { expressions.Add(x => x.Id == filter.Id); }

            if (!string.IsNullOrEmpty(filter.Title)) { expressions.Add(x => x.Title.Contains(filter.Title)); }

            List<OfferHeaderEntity> entities = _dataAccessService.Execute(x => GetBy(x, expressions.ToArray()));

            return _mapper.Map<List<OfferHeaderModel>>(entities);
        }

        public OfferHeaderModel GetById(int id)
        {
            OfferHeaderEntity entity = _dataAccessService.Execute(x => GetById(x, id));
            return _mapper.Map<OfferHeaderModel>(entity);
        }

        public bool SetEnabled(int id, bool enabled)
        {
            return _dataAccessService.Execute(x => GetAndUpdate(x,
                y => y.Id == id,
                y => y.Enabled = enabled)) > 0;
        }

        public bool Update(UpdateOfferHeader updateOfferHeader)
        {
            return _dataAccessService.Execute(x => GetAndUpdate(x,
                y => y.Id == updateOfferHeader.Id,
                y => _mapper.Map(updateOfferHeader, y))) > 0;
        }

        private void DeleteOffersAndHistories(DataConnection db, int offerHeaderId)
        {
            List<int> offerIds = _offersRepository.GetOfferIdsByOfferHeaderId(db, offerHeaderId);

            _offerHistoriesRepository.DeleteByOfferIds(db, offerIds);
            _offersRepository.DeleteMany(db, offerIds);
        }
    }
}