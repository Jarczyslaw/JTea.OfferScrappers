﻿namespace JTea.OfferScrappers.WindowsService.Responses
{
    public class GetConfigurationResponse
    {
        public string CronExpression { get; set; }

        public TimeSpan DelayBetweenOffersChecks { get; set; }

        public TimeSpan DelayBetweenSubPagesChecks { get; set; }
    }
}