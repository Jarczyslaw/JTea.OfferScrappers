namespace JTea.OfferScrappers.WindowsService.Models.Exceptions
{
    public class NoOfferHeadersException : DefinedException
    {
        public NoOfferHeadersException()
            : base($"There are no enabled offer headers")
        {
        }
    }
}