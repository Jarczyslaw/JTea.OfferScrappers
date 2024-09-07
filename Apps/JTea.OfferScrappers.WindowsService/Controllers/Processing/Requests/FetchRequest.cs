namespace JTea.OfferScrappers.WindowsService.Controllers.Processing.Requests
{
    public class FetchRequest
    {
        public ScrapperType OfferType { get; set; }

        public string OfferUrl { get; set; }

        public PageSourceProviderType PageSourceProviderType { get; set; }
    }
}