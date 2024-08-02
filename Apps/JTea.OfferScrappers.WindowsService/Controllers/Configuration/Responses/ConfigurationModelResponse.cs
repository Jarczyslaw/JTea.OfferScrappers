namespace JTea.OfferScrappers.WindowsService.Controllers.Configuration.Responses
{
    public class ConfigurationModelResponse
    {
        public string CronExpression { get; set; }

        public TimeSpan DelayBetweenOffersChecks { get; set; }

        public TimeSpan DelayBetweenSubPagesChecks { get; set; }
    }
}