namespace JTea.OfferScrappers.Logic.Models.Exceptions
{
    public abstract class DefinedException : Exception
    {
        public DefinedException(string message) : base(message)
        {
        }
    }
}