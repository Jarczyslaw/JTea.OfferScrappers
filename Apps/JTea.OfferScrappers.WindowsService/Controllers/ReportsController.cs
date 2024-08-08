using JTea.OfferScrappers.WindowsService.Controllers.Common.Responses;
using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : BaseController
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpPost("filter")]
        public ActionResult<List<OfferHeaderModelResponse>> GetByFilter([FromBody] OfferHeadersFilter filter)
        {
            // TODO
            return Ok();
        }
    }
}