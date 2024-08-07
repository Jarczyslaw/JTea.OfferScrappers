﻿using JToolbox.DataAccess.SQLiteNet.Entities;
using SQLite;

namespace JTea.OfferScrappers.WindowsService.Persistence.Entities
{
    [Table("Configuration")]
    public class ConfigurationEntity : BaseEntity
    {
        public string CronExpression { get; set; }

        public TimeSpan DelayBetweenOffersChecks { get; set; }

        public TimeSpan DelayBetweenSubPagesChecks { get; set; }
    }
}