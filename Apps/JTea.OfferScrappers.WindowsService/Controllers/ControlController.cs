using JTea.OfferScrappers.WindowsService.Scheduling;
using JToolbox.Core.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly ISchedulingService _schedulingService;

        public ControlController(
            ILoggerService loggerService,
            ISchedulingService schedulingService)
        {
            _loggerService = loggerService;
            _schedulingService = schedulingService;
        }

        [HttpGet("ping")]
        public ActionResult<bool> Ping() => true;

        [HttpPost("startNow")]
        public async Task<ActionResult> StartNow()
        {
            await _schedulingService.StartNow();
            return Ok();
        }
    }
}