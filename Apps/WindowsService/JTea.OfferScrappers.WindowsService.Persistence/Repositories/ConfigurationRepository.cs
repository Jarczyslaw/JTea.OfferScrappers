using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.L2DB;
using JToolbox.DataAccess.L2DB.Repositories;
using MapsterMapper;

namespace JTea.OfferScrappers.WindowsService.Persistence.Repositories
{
    public class ConfigurationRepository : BaseRepository<ConfigurationEntity>, IConfigurationRepository
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IMapper _mapper;

        public ConfigurationRepository(
            IMapper mapper,
            IDataAccessService dataAccessService)
        {
            _mapper = mapper;
            _dataAccessService = dataAccessService;
        }

        public ConfigurationModel GetConfiguration()
        {
            ConfigurationEntity entity = _dataAccessService.RunFunction(GetFirstOrDefault);
            return _mapper.Map<ConfigurationModel>(entity);
        }

        public int GetDbVersion() => _dataAccessService.GetDbVersion();

        public void UpdateConfiguration(ConfigurationModel configuration)
        {
            ConfigurationEntity entity = _mapper.Map<ConfigurationEntity>(configuration);
            _dataAccessService.RunActionTransaction(
                x => Update(x, entity));
        }
    }
}