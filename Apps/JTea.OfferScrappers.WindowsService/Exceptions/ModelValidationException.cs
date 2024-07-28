using JTea.OfferScrappers.WindowsService.Abstraction.Exceptions;

namespace JTea.OfferScrappers.WindowsService.Exceptions
{
    public class ModelValidationException : DefinedException
    {
        public ModelValidationException(string message) : base(message)
        {
        }
    }
}