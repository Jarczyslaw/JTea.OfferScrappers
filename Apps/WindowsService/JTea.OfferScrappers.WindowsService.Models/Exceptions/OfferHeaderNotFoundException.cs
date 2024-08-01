namespace JTea.OfferScrappers.WindowsService.Models.Exceptions
{
    public class OfferHeaderNotFoundException : DefinedException
    {
        public OfferHeaderNotFoundException(int offerHeaderId)
            : base($"Offer header with id {offerHeaderId} not found")
        {
        }
    }
}