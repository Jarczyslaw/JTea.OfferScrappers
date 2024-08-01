using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JTea.OfferScrappers.WindowsService.Persistence.Entities;
using JToolbox.DataAccess.SQLiteNet;
using JToolbox.DataAccess.SQLiteNet.Repositories;
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
            ConfigurationEntity entity = _dataAccessService.Execute(GetFirstOrDefault);
            return _mapper.Map<ConfigurationModel>(entity);
        }

        public void UpdateConfiguration(ConfigurationModel configuration)
        {
            ConfigurationEntity entity = _mapper.Map<ConfigurationEntity>(configuration);
            _dataAccessService.ExecuteTransaction(
                x => Update(x, entity));
        }
    }
}