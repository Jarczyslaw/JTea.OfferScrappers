namespace JTea.OfferScrappers.WebHost.Controllers.Common.Responses
{
    public class OfferModelResponse : ScrappedOfferModelResponse
    {
        public List<OfferHistoryModelResponse> Histories { get; set; } = [];

        public int Id { get; set; }

        public int OfferHeaderId { get; set; }
    }
}