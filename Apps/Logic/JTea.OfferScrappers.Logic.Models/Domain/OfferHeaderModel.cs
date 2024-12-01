namespace JTea.OfferScrappers.Logic.Models.Domain
{
    public class OfferHeaderModel : BaseModel
    {
        public bool IsEnabled { get; set; }

        public bool? IsLastCheckValid { get; set; }

        public DateTime? LastCheckDateEnd { get; set; }

        public DateTime? LastCheckDateStart { get; set; }

        public string LastCheckErrorMessage { get; set; }

        public int? LastCheckOffersCount { get; set; }

        public List<OfferModel> Offers { get; set; } = [];

        public string OfferUrl { get; set; }

        public string Title { get; set; }

        public ScrapperType Type { get; set; }

        public void ClearProcessingData()
        {
            LastCheckDateEnd
                = LastCheckDateStart = null;
            LastCheckErrorMessage = null;
            LastCheckOffersCount = null;
            IsLastCheckValid = null;
        }
    }
}