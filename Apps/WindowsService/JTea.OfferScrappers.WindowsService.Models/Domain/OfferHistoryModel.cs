using JTea.OfferScrappers.WindowsService.Models.Domain.Enums;

namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class OfferHistoryModel : BaseModel
    {
        public DateTime Date { get; set; }

        public OfferModel Offer { get; set; }

        public int OfferId { get; set; }

        public OfferState State { get; set; }
    }
}