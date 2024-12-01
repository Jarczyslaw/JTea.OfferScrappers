using JTea.OfferScrappers.Logic.Core.Services.Interfaces;
using JToolbox.Core.Abstraction;
using JToolbox.Misc.QuartzScheduling;
using Quartz;

namespace JTea.OfferScrappers.WebHost.Scheduling
{
    [DisallowConcurrentExecution]
    public class MainJob : IJob
    {
        private readonly ILoggerService _loggerService;
        private readonly IProcessingService _processingService;

        public MainJob(
            ILoggerService loggerService,
            IProcessingService processingService)
        {
            _loggerService = loggerService;
            _processingService = processingService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (QuartzExtensions.IsMisfired(context))
                {
                    _loggerService.Error($"MainJob missfired. Scheduled time: {context.ScheduledFireTimeUtc.Value}, fire time: {context.FireTimeUtc}");
                    return Task.CompletedTask;
                }

                _processingService.ProcessAllOfferHeaders(waitIfCurrentlyProcessing: true);
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "MainJob failed to execute");
            }
            return Task.CompletedTask;
        }
    }
}