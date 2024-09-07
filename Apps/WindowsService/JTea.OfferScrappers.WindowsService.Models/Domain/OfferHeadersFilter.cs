namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class OfferHeadersFilter
    {
        public bool CompleteData { get; set; } = true;

        public int? Id { get; set; }

        public string Title { get; set; }

        public ScrapperType? Type { get; set; }
    }
}