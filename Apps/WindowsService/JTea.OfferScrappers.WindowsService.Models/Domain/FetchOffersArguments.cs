namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class FetchOffersArguments
    {
        public ScrapperType OfferType { get; set; }

        public string OfferUrl { get; set; }

        public PageSourceProviderType PageSourceProviderType { get; set; }
    }
}