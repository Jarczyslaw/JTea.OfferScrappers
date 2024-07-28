namespace JTea.OfferScrappers.WindowsService.Abstraction.Exceptions
{
    public abstract class DefinedException : Exception
    {
        public DefinedException(string message) : base(message)
        {
        }
    }
}