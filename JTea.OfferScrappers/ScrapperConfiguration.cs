using System;

namespace JTea.OfferScrappers
{
    public class ScrapperConfiguration
    {
        public TimeSpan DelayBetweenSubPagesChecks { get; set; } = TimeSpan.FromSeconds(5);

        public int? OffersLimits { get; set; }
    }
}