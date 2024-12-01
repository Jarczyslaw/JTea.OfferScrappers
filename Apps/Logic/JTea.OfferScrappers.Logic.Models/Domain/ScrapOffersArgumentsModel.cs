namespace JTea.OfferScrappers.Logic.Models.Domain
{
    public class ScrapOffersArgumentsModel
    {
        public ScrapperType OfferType { get; set; }

        public string OfferUrl { get; set; }

        public PageSourceProviderType PageSourceProviderType { get; set; }
    }
}