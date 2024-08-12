namespace JTea.OfferScrappers.WindowsService.Controllers.Common.Responses
{
    public class OfferModelResponse
    {
        public string Description { get; set; }

        public List<OfferHistoryModelResponse> Histories { get; set; } = [];

        public int Id { get; set; }

        public string ImageHref { get; set; }

        public int OfferHeaderId { get; set; }

        public string Price { get; set; }

        public string TargetHref { get; set; }

        public string Title { get; set; }
    }
}