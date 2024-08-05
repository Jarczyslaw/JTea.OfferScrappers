using JTea.OfferScrappers.WindowsService.Abstraction.Services;
using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessingController : BaseController
    {
        private readonly IProcessingService _processingService;
        private readonly ISchedulingService _schedulingService;

        public ProcessingController(
            ISchedulingService schedulingService,
            IProcessingService processingService)
        {
            _schedulingService = schedulingService;
            _processingService = processingService;
        }

        [HttpPost("triggerNow")]
        public async Task<ActionResult> TriggerNow()
        {
            await _schedulingService.StartNow();

            return Ok();
        }
    }
}