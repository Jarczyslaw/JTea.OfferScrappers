using JTea.OfferScrappers.Abstraction;
using System;

namespace JTea.OfferScrappers
{
    public class ScrapperConfiguration
    {
        public bool CheckSubpages { get; set; } = true;

        public TimeSpan DelayBetweenSubPagesChecks { get; set; } = TimeSpan.FromSeconds(5);

        public int? OffersLimit { get; set; }

        public string PageSourceLogPath { get; set; }

        public IPageSourceProvider PageSourceProvider { get; set; }
    }
}