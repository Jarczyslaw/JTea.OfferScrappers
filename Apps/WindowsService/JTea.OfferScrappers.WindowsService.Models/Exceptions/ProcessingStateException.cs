using JTea.OfferScrappers.WindowsService.Models.Domain;

namespace JTea.OfferScrappers.WindowsService.Models.Exceptions
{
    public class ProcessingStateException : DefinedException
    {
        public ProcessingStateException()
            : base($"Action can not be performed because processing service is now in {ProcessingState.Running} state")
        {
        }
    }
}