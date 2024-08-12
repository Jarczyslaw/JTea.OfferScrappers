using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Controllers.Common.Responses
{
    public class OfferHistoryModelResponse
    {
        public DateTime Date { get; set; }

        public OfferState State { get; set; }
    }
}