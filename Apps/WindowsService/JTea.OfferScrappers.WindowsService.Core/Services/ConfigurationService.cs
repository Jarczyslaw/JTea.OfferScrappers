using JTea.OfferScrappers.WindowsService.Abstraction.Exceptions;
using JTea.OfferScrappers.WindowsService.Abstraction.Services;
using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JToolbox.Core.Abstraction;
using MapsterMapper;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private static readonly SemaphoreSlim _configurationSemaphore = new(1, 1);
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        private readonly ISchedulingService _schedulingService;

        public ConfigurationService(
            IMapper mapper,
            ILoggerService loggerService,
            ISchedulingService schedulingService,
            IConfigurationRepository configurationRepository)
        {
            _mapper = mapper;
            _loggerService = loggerService;
            _schedulingService = schedulingService;
            _configurationRepository = configurationRepository;
        }

        public Configuration GetConfiguration() => _configurationRepository.GetConfiguration();

        public Task StartNow() => _schedulingService.StartNow();

        public async Task UpdateConfiguration(Configuration newConfiguration)
        {
            try
            {
                await _configurationSemaphore.WaitAsync();

                Configuration existingConfiguration = _configurationRepository.GetConfiguration();

                bool rescheduleRequired = false;
                if (existingConfiguration.CronExpression != newConfiguration.CronExpression)
                {
                    if (!_schedulingService.IsValidCronExpression(newConfiguration.CronExpression))
                    {
                        throw new InvalidCronExpressionException(newConfiguration.CronExpression);
                    }

                    rescheduleRequired = true;
                }

                newConfiguration.Id = existingConfiguration.Id;
                _configurationRepository.UpdateConfiguration(newConfiguration);

                if (rescheduleRequired)
                {
                    await _schedulingService.StartWithCron(newConfiguration.CronExpression);
                    _loggerService.Info($"Quartz rescheduled with cron expression: {newConfiguration.CronExpression}");
                }
            }
            finally
            {
                _configurationSemaphore.Release();
            }
        }
    }
}