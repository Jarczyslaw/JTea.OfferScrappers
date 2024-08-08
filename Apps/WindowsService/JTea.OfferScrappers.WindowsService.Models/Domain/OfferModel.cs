namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class OfferModel : BaseModel
    {
        public string Description { get; set; }

        public List<OfferHistoryModel> Histories { get; set; } = [];

        public string ImageHref { get; set; }

        public OfferHistoryModel LastHistory => Histories.Count == 0
            ? null
            : Histories.MaxBy(x => x.Date);

        public OfferHeaderModel OfferHeader { get; set; }

        public int OfferHeaderId { get; set; }

        public string Price { get; set; }

        public string TargetHref { get; set; }

        public string Title { get; set; }
    }
}