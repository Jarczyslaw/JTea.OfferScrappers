namespace JTea.OfferScrappers.Logic.Models.Exceptions
{
    public class ProcessingStateException : DefinedException
    {
        public ProcessingStateException()
            : base($"Action can not be performed because processing service is now in running state")
        {
        }
    }
}