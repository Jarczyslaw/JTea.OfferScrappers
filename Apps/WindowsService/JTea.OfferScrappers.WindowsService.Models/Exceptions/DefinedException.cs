namespace JTea.OfferScrappers.WindowsService.Models.Exceptions
{
    public abstract class DefinedException : Exception
    {
        public DefinedException(string message) : base(message)
        {
        }
    }
}