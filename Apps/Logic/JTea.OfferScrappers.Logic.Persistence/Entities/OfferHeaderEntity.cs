using JToolbox.DataAccess.L2DB.SQLite.Entities;
using LinqToDB.Mapping;

namespace JTea.OfferScrappers.Logic.Persistence.Entities
{
    [Table("OfferHeaders")]
    public class OfferHeaderEntity : BaseEntity
    {
        [Column, NotNull]
        public bool IsEnabled { get; set; }

        [Column]
        public bool? IsLastCheckValid { get; set; }

        [Column]
        public DateTime? LastCheckDateEnd { get; set; }

        [Column]
        public DateTime? LastCheckDateStart { get; set; }

        [Column]
        public string LastCheckErrorMessage { get; set; }

        [Column]
        public int? LastCheckOffersCount { get; set; }

        [Association(ThisKey = nameof(Id), OtherKey = nameof(OfferEntity.OfferHeaderId))]
        public List<OfferEntity> Offers { get; set; } = [];

        [Column, NotNull]
        public string OfferUrl { get; set; }

        [Column, NotNull]
        public string Title { get; set; }

        [Column, NotNull]
        public ScrapperType Type { get; set; }
    }
}