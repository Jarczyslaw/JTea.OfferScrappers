using JToolbox.DataAccess.SQLiteNet.Entities;
using SQLite;

namespace JTea.OfferScrappers.WindowsService.Persistence.Entities
{
    [Table("Offers")]
    public class OfferEntity : BaseEntity
    {
        public string Description { get; set; }

        [Ignore]
        public List<OfferHistoryEntity> Histories { get; set; } = [];

        public string ImageHref { get; set; }

        [Ignore]
        public OfferHeaderEntity OfferHeader { get; set; }

        public int OfferHeaderId { get; set; }

        public string Price { get; set; }

        public string TargetHref { get; set; }

        public string Title { get; set; }
    }
}