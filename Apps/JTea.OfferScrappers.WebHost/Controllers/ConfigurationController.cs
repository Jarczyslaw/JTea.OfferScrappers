﻿using JTea.OfferScrappers.Logic.Core.Services.Interfaces;
using JTea.OfferScrappers.Logic.Models.Domain;
using JTea.OfferScrappers.WebHost.Controllers.Configuration.Requests;
using JTea.OfferScrappers.WebHost.Controllers.Configuration.Responses;
using JToolbox.Core.Models.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WebHost.Controllers
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

        [HttpGet("dbVersion")]
        public ActionResult<int> GetDbVersion() => Ok(_configurationService.GetDbVersion());

        [HttpPut]
        public async Task<ActionResult<ConfigurationModelResponse>> UpdateConfiguration([FromBody] UpdateConfigurationRequest request)
        {
            CheckModel();

            ConfigurationModel configuration = _mapper.Map<ConfigurationModel>(request);
            Result<ConfigurationModel> result = await _configurationService.UpdateConfiguration(configuration);

            return CreateActionResult(result, _mapper.Map<ConfigurationModelResponse>);
        }
    }
}