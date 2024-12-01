namespace JTea.OfferScrappers.Logic.Abstraction.Services
{
    public interface ISchedulingService
    {
        Task Initialize();

        bool IsValidCronExpression(string cronExpression);

        Task StartNow();

        Task StartWithCron(string cronExpression);

        Task Stop();
    }
}