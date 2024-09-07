namespace JTea.OfferScrappers.WindowsService.Controllers.Common.Responses
{
    public class ScrapResultModelResponse
    {
        public List<ScrappedOfferModelResponse> Offers { get; set; }

        public int OffersCount { get; set; }

        public string OffersCountText { get; set; }

        public int ValidOffers { get; set; }
    }
}