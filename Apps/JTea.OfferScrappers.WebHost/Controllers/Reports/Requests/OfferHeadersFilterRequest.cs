namespace JTea.OfferScrappers.WebHost.Controllers.Reports.Requests
{
    public class OfferHeadersFilterRequest
    {
        public bool CompleteData { get; set; } = true;

        public int? Id { get; set; }

        public string Title { get; set; }

        public ScrapperType? Type { get; set; }
    }
}