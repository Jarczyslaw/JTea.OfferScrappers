using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JTea.OfferScrappers.WindowsService.Controllers.Common.Responses
{
    public class OfferHeaderModelResponse
    {
        public int Id { get; set; }

        public bool IsEnabled { get; set; }

        public bool? IsLastCheckValid { get; set; }

        public DateTime? LastCheckDateEnd { get; set; }

        public DateTime? LastCheckDateStart { get; set; }

        public string LastCheckErrorMessage { get; set; }

        public int? LastCheckOffersCount { get; set; }

        public List<OfferModelResponse> Offers { get; set; } = new();

        public string OfferUrl { get; set; }

        public string Title { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ScrapperType Type { get; set; }
    }
}