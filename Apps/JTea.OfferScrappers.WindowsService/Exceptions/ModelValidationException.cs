using JTea.OfferScrappers.WindowsService.Models.Exceptions;

namespace JTea.OfferScrappers.WindowsService.Exceptions
{
    public class ModelValidationException : DefinedException
    {
        public ModelValidationException(string message) : base(message)
        {
        }
    }
}