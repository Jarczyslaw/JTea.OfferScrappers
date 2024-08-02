namespace JTea.OfferScrappers.WindowsService.Controllers.Configuration.Requests
{
    public class UpdateConfigurationRequest
    {
        public string CronExpression { get; set; }

        public TimeSpan DelayBetweenOffersChecks { get; set; }

        public TimeSpan DelayBetweenSubPagesChecks { get; set; }
    }
}