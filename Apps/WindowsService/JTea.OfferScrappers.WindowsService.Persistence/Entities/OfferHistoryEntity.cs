using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.DataAccess.SQLiteNet.Entities;
using SQLite;

namespace JTea.OfferScrappers.WindowsService.Persistence.Entities
{
    [Table("OfferHistories")]
    public class OfferHistoryEntity : BaseEntity
    {
        public DateTime Date { get; set; }

        [Ignore]
        public OfferEntity Offer { get; set; }

        public int OfferId { get; set; }

        public OfferState State { get; set; }
    }
}