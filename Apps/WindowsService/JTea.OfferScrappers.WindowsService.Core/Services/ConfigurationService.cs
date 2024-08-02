﻿using JTea.OfferScrappers.WindowsService.Abstraction.Services;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Models.Exceptions;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JToolbox.Core.Abstraction;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private static readonly SemaphoreSlim _configurationSemaphore = new(1, 1);
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILoggerService _loggerService;
        private readonly ISchedulingService _schedulingService;

        public ConfigurationService(
            ILoggerService loggerService,
            ISchedulingService schedulingService,
            IConfigurationRepository configurationRepository)
        {
            _loggerService = loggerService;
            _schedulingService = schedulingService;
            _configurationRepository = configurationRepository;
        }

        public ConfigurationModel GetConfiguration() => _configurationRepository.GetConfiguration();

        public Task StartNow() => _schedulingService.StartNow();

        public async Task<Result<ConfigurationModel>> UpdateConfiguration(ConfigurationModel newConfiguration)
        {
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