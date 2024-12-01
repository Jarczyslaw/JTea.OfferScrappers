using JTea.OfferScrappers.Logic.Models.Domain.Enums;

namespace JTea.OfferScrappers.WebHost.Controllers.Common.Responses
{
    public class OfferHistoryModelResponse
    {
        public DateTime Date { get; set; }

        public OfferState State { get; set; }
    }
}