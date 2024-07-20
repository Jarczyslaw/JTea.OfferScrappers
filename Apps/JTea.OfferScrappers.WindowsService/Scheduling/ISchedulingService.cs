namespace JTea.OfferScrappers.WindowsService.Scheduling
{
    public interface ISchedulingService
    {
        Task Initialize();

        Task StartNow();

        Task Stop();
    }
}