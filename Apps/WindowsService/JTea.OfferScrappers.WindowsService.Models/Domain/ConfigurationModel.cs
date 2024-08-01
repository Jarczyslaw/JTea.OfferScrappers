namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class ConfigurationModel : BaseModel
    {
        public string CronExpression { get; set; }

        public TimeSpan DelayBetweenOffersChecks { get; set; }

        public TimeSpan DelayBetweenSubPagesChecks { get; set; }
    }
}