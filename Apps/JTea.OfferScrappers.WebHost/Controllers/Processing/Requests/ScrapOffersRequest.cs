namespace JTea.OfferScrappers.WebHost.Controllers.Processing.Requests
{
    public class ScrapOffersRequest
    {
        public ScrapperType OfferType { get; set; }

        public string OfferUrl { get; set; }

        public PageSourceProviderType PageSourceProviderType { get; set; }
    }
}