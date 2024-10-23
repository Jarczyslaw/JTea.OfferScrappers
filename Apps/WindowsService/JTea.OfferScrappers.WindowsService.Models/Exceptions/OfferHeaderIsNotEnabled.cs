namespace JTea.OfferScrappers.WindowsService.Models.Exceptions
{
    public class OfferHeaderIsNotEnabled : DefinedException
    {
        public OfferHeaderIsNotEnabled(int offerHeaderId)
            : base($"Offer header with id {offerHeaderId} is not enabled")
        {
        }
    }
}