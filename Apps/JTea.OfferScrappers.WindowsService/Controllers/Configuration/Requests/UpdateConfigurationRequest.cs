﻿namespace JTea.OfferScrappers.WindowsService.Controllers.Configuration.Requests
{
    public class UpdateConfigurationRequest
    {
        public string CronExpression { get; set; }

        public int DelayBetweenOffersChecksSeconds { get; set; }

        public int DelayBetweenSubPagesChecksSeconds { get; set; }
    }
}