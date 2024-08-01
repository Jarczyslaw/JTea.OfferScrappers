namespace JTea.OfferScrappers.WindowsService.Models
{
    public class OfferHeadersFilter
    {
        public bool? Enabled { get; set; }

        public string Title { get; set; }

        public ScrapperType? Type { get; set; }
    }
}