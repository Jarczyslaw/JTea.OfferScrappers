using JTea.OfferScrappers.WindowsService.Models;

namespace JTea.OfferScrappers.WindowsService.Core.Services.Interfaces
{
    public interface IProcessingService
    {
        ProcessingState State { get; }
    }
}