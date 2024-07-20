using JToolbox.Core.Abstraction;
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

            _loggerService.Info($"{nameof(MainJob)} initialized properly");
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}