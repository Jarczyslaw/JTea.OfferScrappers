namespace JTea.OfferScrappers.Logic.Models.Exceptions
{
    public class NoOfferHeadersException : DefinedException
    {
        public NoOfferHeadersException()
            : base($"There are no enabled offer headers")
        {
        }
    }
}