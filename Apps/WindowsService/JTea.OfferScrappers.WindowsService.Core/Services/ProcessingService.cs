using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class ProcessingService : IProcessingService
    {
        public ProcessingService()
        {
        }

        public ProcessingState State { get; private set; }
    }
}