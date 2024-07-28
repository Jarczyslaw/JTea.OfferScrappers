using JTea.OfferScrappers.WindowsService.Core.Services;
using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Requests;
using JTea.OfferScrappers.WindowsService.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : BaseController
    {
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;

        public ConfigurationController(
            IConfigurationService configurationService,
            IMapper mapper)
        {
            _configurationService = configurationService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<GetConfigurationResponse> GetConfiguration()
        {
            Configuration configuration = _configurationService.GetConfiguration();

            return Ok(_mapper.Map<GetConfigurationResponse>(configuration));
        }

        [HttpGet("ping")]
        public ActionResult<bool> Ping() => Ok(true);

        [HttpPost("startNow")]
        public async Task<ActionResult> StartNow()
        {
            await _configurationService.StartNow();

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateConfiguration([FromBody] UpdateConfigurationRequest request)
        {
            CheckModel();

            Configuration configuration = _mapper.Map<Configuration>(request);
            await _configurationService.UpdateConfiguration(configuration);

            return Ok();
        }
    }
}