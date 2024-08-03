namespace JTea.OfferScrappers.WindowsService.Models
{
    public class OfferHeadersFilter
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public ScrapperType? Type { get; set; }
    }
}