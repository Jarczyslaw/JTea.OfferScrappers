using JToolbox.Core.Abstraction;
using JToolbox.Misc.QuartzScheduling;
using Quartz;

namespace JTea.OfferScrappers.WindowsService.Scheduling
{
    [DisallowConcurrentExecution]
    public class MainJob : IJob
    {
        private readonly ILoggerService _loggerService;

        public MainJob(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (QuartzExtensions.IsMisfired(context)) { return Task.CompletedTask; }
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "MainJob failed to execute");
            }
            return Task.CompletedTask;
        }
    }
}