namespace JTea.OfferScrappers.WindowsService.Models.Domain
{
    public class ProcessingResultModel
    {
        public ProcessingResultModel(OfferHeaderModel offerHeaderModel)
        {
            CheckDuration = offerHeaderModel.LastCheckDateStart != null && offerHeaderModel.LastCheckDateEnd != null
                ? offerHeaderModel.LastCheckDateEnd.Value - offerHeaderModel.LastCheckDateStart.Value
                : null;

            // TODO JTJT dopisać resztę i zwrócić wynik
        }

        public TimeSpan? CheckDuration { get; set; }

        public string ErrorMessage { get; set; }

        public bool? IsCheckValid { get; set; }

        public int OfferHeaderId { get; set; }

        public int? OffersCount { get; set; }

        public string OfferUrl { get; set; }

        public string Title { get; set; }

        public ScrapperType Type { get; set; }
    }
}