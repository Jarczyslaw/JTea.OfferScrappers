using JToolbox.DataAccess.SQLiteNet.Entities;
using SQLite;

namespace JTea.OfferScrappers.WindowsService.Persistence.Entities
{
    [Table("OfferHeaders")]
    public class OfferHeaderEntity : BaseEntity
    {
        public bool Enabled { get; set; }

        public DateTime? LastCheckDateEnd { get; set; }

        public DateTime? LastCheckDateStart { get; set; }

        [Ignore]
        public List<OfferEntity> Offers { get; set; } = [];

        public string OfferUrl { get; set; }

        public string Title { get; set; }

        public ScrapperType Type { get; set; }
    }
}