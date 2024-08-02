using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.SQLiteNet;
using JToolbox.DataAccess.SQLiteNet.Repositories;
using MapsterMapper;
using System.Linq.Expressions;

namespace JTea.OfferScrappers.WindowsService.Persistence.Repositories
{
    public class OfferHeadersRepository : BaseRepository<OfferHeaderEntity>, IOfferHeadersRepository
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IMapper _mapper;

        public OfferHeadersRepository(
            IMapper mapper,
            IDataAccessService dataAccessService)
        {
            _mapper = mapper;
            _dataAccessService = dataAccessService;
        }

        public OfferHeaderModel Create(OfferHeaderModel offerHeader)
        {
            OfferHeaderEntity entity = _mapper.Map<OfferHeaderEntity>(offerHeader);
            offerHeader.Id = _dataAccessService.Execute(x => Create(x, entity));
            return offerHeader;
        }

        public bool Delete(int id)
        {
            return _dataAccessService.Execute(x => Delete(x, id));
        }

        public void DeleteAll()
        {
            _dataAccessService.Execute(x => x.Execute($"DELETE FROM {SqliteExtensions.GetTableName(typeof(OfferHeaderEntity))}"));
        }

        public List<OfferHeaderModel> GetAll()
        {
            List<OfferHeaderEntity> entities = _dataAccessService.Execute(GetAll);
            return _mapper.Map<List<OfferHeaderModel>>(entities);
        }

        public List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter)
        {
            List<Expression<Func<OfferHeaderEntity, bool>>> expressions = new();

            if (filter.Type != null) { expressions.Add(x => x.Type == filter.Type); }

            if (filter.Enabled != null) { expressions.Add(x => x.Enabled == filter.Enabled); }

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
    }
}