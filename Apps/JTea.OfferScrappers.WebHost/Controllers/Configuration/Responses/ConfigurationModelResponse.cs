﻿namespace JTea.OfferScrappers.WebHost.Controllers.Configuration.Responses
{
    public class ConfigurationModelResponse
    {
        public string CronExpression { get; set; }

        public int DelayBetweenOffersChecksSeconds { get; set; }

        public int DelayBetweenSubPagesChecksSeconds { get; set; }
    }
}