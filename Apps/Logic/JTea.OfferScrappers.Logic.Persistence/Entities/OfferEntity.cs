using JToolbox.DataAccess.L2DB.SQLite.Entities;
using LinqToDB.Mapping;

namespace JTea.OfferScrappers.Logic.Persistence.Entities
{
    [Table("Offers")]
    public class OfferEntity : BaseEntity
    {
        [Column]
        public string Description { get; set; }

        [Association(ThisKey = nameof(Id), OtherKey = nameof(OfferHistoryEntity.OfferId))]
        public List<OfferHistoryEntity> Histories { get; set; } = [];

        [Column]
        public string ImageHref { get; set; }

        [Association(ThisKey = nameof(OfferHeaderId), OtherKey = nameof(OfferHeaderEntity.Id))]
        public OfferHeaderEntity OfferHeader { get; set; }

        [Column, NotNull]
        public int OfferHeaderId { get; set; }

        [Column]
        public string Price { get; set; }

        [Column, NotNull]
        public string TargetHref { get; set; }

        [Column, NotNull]
        public string Title { get; set; }
    }
}