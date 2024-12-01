using JTea.OfferScrappers.Logic.Models.Domain;
using JTea.OfferScrappers.Logic.Persistence.Abstraction;
using JTea.OfferScrappers.Logic.Persistence.Entities;
using JToolbox.DataAccess.L2DB;
using JToolbox.DataAccess.L2DB.Repositories;
using LinqToDB;
using LinqToDB.Data;
using MapsterMapper;

namespace JTea.OfferScrappers.Logic.Persistence.Repositories
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

        public bool Clear(OfferHeaderModel offerHeader)
        {
            OfferHeaderEntity entity = _mapper.Map<OfferHeaderEntity>(offerHeader);

            return _dataAccessService.RunFunctionTransaction(x =>
            {
                Update(x, entity);
                DeleteOffersAndHistories(x, offerHeader.Id);

                return true;
            });
        }

        public OfferHeaderModel Create(OfferHeaderModel offerHeader)
        {
            OfferHeaderEntity entity = _mapper.Map<OfferHeaderEntity>(offerHeader);
            offerHeader.Id = _dataAccessService.RunFunction(x => Create(x, entity));
            return offerHeader;
        }

        public bool Delete(int offerHeaderId)
        {
            return _dataAccessService.RunFunctionTransaction(x =>
            {
                DeleteOffersAndHistories(x, offerHeaderId);
                return Delete(x, offerHeaderId);
            });
        }

        public void DeleteAll()
        {
            _dataAccessService.RunActionTransaction(x =>
            {
                x.Execute($"DELETE FROM {L2DbExtensions.GetTableName(typeof(OfferHistoryEntity))}");
                x.Execute($"DELETE FROM {L2DbExtensions.GetTableName(typeof(OfferEntity))}");
                x.Execute($"DELETE FROM {L2DbExtensions.GetTableName(typeof(OfferHeaderEntity))}");
            });
        }

        public List<OfferHeaderModel> GetAll(bool completeData)
        {
            List<OfferHeaderEntity> entities = _dataAccessService.RunFunction(x =>
            {
                IQueryable<OfferHeaderEntity> query = Table(x);

                if (completeData)
                {
                    query = query.LoadWith(x => x.Offers)
                        .ThenLoad(x => x.Histories);
                }

                return query.ToList();
            });

            OrderHistories(entities);

            return _mapper.Map<List<OfferHeaderModel>>(entities);
        }

        public List<OfferHeaderModel> GetByFilter(OfferHeadersFilterModel filter)
        {
            List<OfferHeaderEntity> entities = _dataAccessService.RunFunction(x =>
            {
                IQueryable<OfferHeaderEntity> query = Table(x);

                if (filter.CompleteData)
                {
                    query = query.LoadWith(x => x.Offers)
                        .ThenLoad(x => x.Histories);
                }

                if (filter.Id != null)
                {
                    query = query.Where(x => x.Id == filter.Id.Value);
                }
                else if (filter.Type != null)
                {
                    query = query.Where(x => x.Type == filter.Type.Value);
                }
                else if (!string.IsNullOrEmpty(filter.Title))
                {
                    query = query.Where(x => x.Title.Contains(filter.Title));
                }

                return query.ToList();
            });

            OrderHistories(entities);

            return _mapper.Map<List<OfferHeaderModel>>(entities);
        }

        public OfferHeaderModel GetById(int id, bool completeData)
        {
            OfferHeaderEntity entity = _dataAccessService.RunFunction(x =>
            {
                IQueryable<OfferHeaderEntity> query = Table(x);

                if (completeData)
                {
                    query = query.LoadWith(x => x.Offers)
                        .ThenLoad(x => x.Histories);
                }

                return query.FirstOrDefault(x => x.Id == id);
            });

            OrderHistories(entity);

            return _mapper.Map<OfferHeaderModel>(entity);
        }

        public bool SetEnabled(int id, bool enabled)
        {
            return _dataAccessService.RunFunction(x => GetAndUpdate(x,
                y => y.Id == id,
                y => y.IsEnabled = enabled)) > 0;
        }

        public bool Update(UpdateOfferHeaderModel updateOfferHeader)
        {
            return _dataAccessService.RunFunction(x => GetAndUpdate(x,
                y => y.Id == updateOfferHeader.Id,
                y => _mapper.Map(updateOfferHeader, y))) > 0;
        }

        private void DeleteOffersAndHistories(DataConnection db, int offerHeaderId)
        {
            List<int> offerIds = _offersRepository.GetOfferIdsByOfferHeaderId(db, offerHeaderId);

            _offerHistoriesRepository.DeleteByOfferIds(db, offerIds);
            _offersRepository.DeleteMany(db, offerIds);
        }

        private void OrderHistories(OfferHeaderEntity entity)
        {
            entity?.Offers?.ForEach(
                y => y.Histories = y.Histories?.OrderBy(x => x.Id).ToList());
        }

        private void OrderHistories(List<OfferHeaderEntity> entities)
        {
            entities.ForEach(OrderHistories);
        }
    }
}