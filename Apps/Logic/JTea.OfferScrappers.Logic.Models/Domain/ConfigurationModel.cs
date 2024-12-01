namespace JTea.OfferScrappers.Logic.Models.Domain
{
    public class ConfigurationModel : BaseModel
    {
        public string CronExpression { get; set; }

        public int DelayBetweenOffersChecksSeconds { get; set; }

        public int DelayBetweenSubPagesChecksSeconds { get; set; }
    }
}