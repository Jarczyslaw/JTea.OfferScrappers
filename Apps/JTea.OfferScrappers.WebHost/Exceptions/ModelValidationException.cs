using JTea.OfferScrappers.Logic.Models.Exceptions;

namespace JTea.OfferScrappers.WebHost.Exceptions
{
    public class ModelValidationException : DefinedException
    {
        public ModelValidationException(string message) : base(message)
        {
        }
    }
}