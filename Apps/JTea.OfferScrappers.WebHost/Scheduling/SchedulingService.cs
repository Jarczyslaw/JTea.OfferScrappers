using JTea.OfferScrappers.Logic.Abstraction.Services;
using JToolbox.Misc.QuartzScheduling;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace JTea.OfferScrappers.WebHost.Scheduling
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

        private TriggerKey CronTriggerKey => new(nameof(StartWithCron));

        private JobKey MainJobKey => nameof(MainJob).ToJobKey();

        private TriggerKey StartNowTriggerKey => new(nameof(StartNow));

        public async Task Initialize()
        {
            await _scheduler.Start();

            IJobDetail jobDetail = SchedulingHelper.CreateJobDetail<MainJob>(MainJobKey, x => x.StoreDurably());

            await _scheduler.AddJob(jobDetail, true);
        }

        public bool IsValidCronExpression(string cronExpression) => CronExpression.IsValidExpression(cronExpression);

        public async Task StartNow()
        {
            ITrigger startNowTrigger = SchedulingHelper.CreateStartNowTrigger(
                MainJobKey,
                StartNowTriggerKey);

            await SchedulingHelper.Schedule(_scheduler, startNowTrigger);
        }

        public async Task StartWithCron(string cronExpression)
        {
            await SchedulingHelper.UnscheduleJob(_scheduler, MainJobKey);

            ITrigger cronTrigger = SchedulingHelper.CreateCronTrigger(
                MainJobKey,
                CronTriggerKey,
                cronExpression);

            await SchedulingHelper.Schedule(_scheduler, cronTrigger);
        }

        public Task Stop()
        {
            return _scheduler.Shutdown(false);
        }
    }
}