using JTea.OfferScrappers.Logic.Models.Domain.Enums;
using JToolbox.DataAccess.L2DB.SQLite.Entities;
using LinqToDB.Mapping;

namespace JTea.OfferScrappers.Logic.Persistence.Entities
{
    [Table("OfferHistories")]
    public class OfferHistoryEntity : BaseEntity
    {
        [Column, NotNull]
        public DateTime Date { get; set; }

        [Association(ThisKey = nameof(OfferId), OtherKey = nameof(OfferEntity.Id))]
        public OfferEntity Offer { get; set; }

        [Column, NotNull]
        public int OfferId { get; set; }

        [Column, NotNull]
        public OfferState State { get; set; }
    }
}