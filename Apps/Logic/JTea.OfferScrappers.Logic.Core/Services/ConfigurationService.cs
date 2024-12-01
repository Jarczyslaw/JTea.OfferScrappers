using JTea.OfferScrappers.Logic.Abstraction.Services;
using JTea.OfferScrappers.Logic.Core.Services.Interfaces;
using JTea.OfferScrappers.Logic.Models.Domain;
using JTea.OfferScrappers.Logic.Models.Exceptions;
using JTea.OfferScrappers.Logic.Persistence.Abstraction;
using JToolbox.Core.Abstraction;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.Logic.Core.Services
{
    public class ConfigurationService : BaseService, IConfigurationService
    {
        private static readonly SemaphoreSlim _configurationSemaphore = new(1, 1);
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILoggerService _loggerService;
        private readonly IProcessingService _processingService;
        private readonly ISchedulingService _schedulingService;

        public ConfigurationService(
            ILoggerService loggerService,
            ISchedulingService schedulingService,
            IConfigurationRepository configurationRepository,
            IProcessingService processingService)
        {
            _loggerService = loggerService;
            _schedulingService = schedulingService;
            _configurationRepository = configurationRepository;
            _processingService = processingService;
        }

        public ConfigurationModel GetConfiguration() => _configurationRepository.GetConfiguration();

        public int GetDbVersion() => _configurationRepository.GetDbVersion();

        public async Task<Result<ConfigurationModel>> UpdateConfiguration(ConfigurationModel newConfiguration)
        {
            if (_processingService.IsRunning) { return GetProcessingStateResult<ConfigurationModel>(); }

            try
            {
                await _configurationSemaphore.WaitAsync();

                ConfigurationModel existingConfiguration = _configurationRepository.GetConfiguration();

                bool rescheduleRequired = false;
                if (existingConfiguration.CronExpression != newConfiguration.CronExpression)
                {
                    if (!_schedulingService.IsValidCronExpression(newConfiguration.CronExpression))
                    {
                        return Result<ConfigurationModel>.AsError(new InvalidCronExpressionException(newConfiguration.CronExpression));
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

                return new Result<ConfigurationModel>(newConfiguration);
            }
            finally
            {
                _configurationSemaphore.Release();
            }
        }
    }
}