namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class OfferHeaderModel : BaseModel
    {
        public bool Enabled { get; set; }

        public DateTime? LastCheckDateEnd { get; set; }

        public DateTime? LastCheckDateStart { get; set; }

        public List<OfferModel> Offers { get; set; } = [];

        public string OfferUrl { get; set; }

        public string Title { get; set; }

        public ScrapperType Type { get; set; }
    }
}