using JToolbox.DataAccess.L2DB.SQLite.Entities;
using LinqToDB.Mapping;

namespace JTea.OfferScrappers.WindowsService.Persistence.Entities
{
    [Table("Configuration")]
    public class ConfigurationEntity : BaseEntity
    {
        [Column, NotNull]
        public string CronExpression { get; set; }

        [Column, NotNull]
        public int DelayBetweenOffersChecksSeconds { get; set; }

        [Column, NotNull]
        public int DelayBetweenSubPagesChecksSeconds { get; set; }
    }
}