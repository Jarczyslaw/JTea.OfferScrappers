using JToolbox.Misc.QuartzScheduling;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace JTea.OfferScrappers.WindowsService.Scheduling
{
    public class SchedulingService : ISchedulingService
    {
        private readonly IScheduler _scheduler;

        public SchedulingService(IJobFactory jobFactory)
        {
            _scheduler = new StdSchedulerFactory()
                .GetScheduler()
                .Result;
            _scheduler.JobFactory = jobFactory;
        }

        private JobKey MainJobKey => nameof(MainJob).ToJobKey();

        public async Task Initialize()
        {
            await _scheduler.Start();

            IJobDetail jobDetail = SchedulingHelper.CreateJobDetail<MainJob>(MainJobKey, x => x.StoreDurably());

            await _scheduler.AddJob(jobDetail, true);
        }

        public async Task StartNow()
        {
            ITrigger startNowTrigger = SchedulingHelper.CreateStartNowTrigger(MainJobKey, new TriggerKey(nameof(StartNow)));
            await SchedulingHelper.Schedule(_scheduler, startNowTrigger);
        }

        public Task Stop()
        {
            return _scheduler.Shutdown(false);
        }
    }
}