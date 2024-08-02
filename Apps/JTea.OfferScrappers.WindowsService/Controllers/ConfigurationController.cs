using JTea.OfferScrappers.WindowsService.Controllers.Configuration.Requests;
using JTea.OfferScrappers.WindowsService.Controllers.Configuration.Responses;
using JTea.OfferScrappers.WindowsService.Core.Services;
using JTea.OfferScrappers.WindowsService.Models.Domain;
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
        public ActionResult<ConfigurationModelResponse> GetConfiguration()
        {
            ConfigurationModel configuration = _configurationService.GetConfiguration();

            return Ok(_mapper.Map<ConfigurationModelResponse>(configuration));
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

            ConfigurationModel configuration = _mapper.Map<ConfigurationModel>(request);
            await _configurationService.UpdateConfiguration(configuration);

            return Ok();
        }
    }
}