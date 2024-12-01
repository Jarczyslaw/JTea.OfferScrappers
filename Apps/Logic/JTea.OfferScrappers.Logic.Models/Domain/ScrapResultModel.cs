namespace JTea.OfferScrappers.Logic.Models.Domain
{
    public class ScrapResultModel
    {
        public List<OfferModel> Offers { get; set; }

        public int OffersCount { get; set; }

        public string OffersCountText { get; set; }

        public int ValidOffers { get; set; }
    }
}