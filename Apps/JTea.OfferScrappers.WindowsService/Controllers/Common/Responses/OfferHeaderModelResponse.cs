using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JTea.OfferScrappers.WindowsService.Controllers.Common.Responses
{
    public class OfferHeaderModelResponse
    {
        public bool Enabled { get; set; }

        public int Id { get; set; }

        public DateTime? LastCheckDateEnd { get; set; }

        public DateTime? LastCheckDateStart { get; set; }

        public string OfferUrl { get; set; }

        public string Title { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ScrapperType Type { get; set; }
    }
}