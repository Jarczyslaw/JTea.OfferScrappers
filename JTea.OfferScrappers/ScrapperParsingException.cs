using System;

namespace JTea.OfferScrappers
{
    public class ScrapperParsingException : Exception
    {
        public ScrapperParsingException()
        {
        }

        public ScrapperParsingException(string message)
            : base(message)
        {
        }

        public ScrapperParsingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}