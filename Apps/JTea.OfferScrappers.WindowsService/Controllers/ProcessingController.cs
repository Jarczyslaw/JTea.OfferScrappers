using JTea.OfferScrappers.WindowsService.Abstraction.Services;
using JTea.OfferScrappers.WindowsService.Controllers.Common.Responses;
using JTea.OfferScrappers.WindowsService.Controllers.Processing.Requests;
using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessingController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProcessingService _processingService;
        private readonly ISchedulingService _schedulingService;

        public ProcessingController(
            IMapper mapper,
            ISchedulingService schedulingService,
            IProcessingService processingService)
        {
            _mapper = mapper;
            _schedulingService = schedulingService;
            _processingService = processingService;
        }

        [HttpPost("fetch")]
        public async Task<ActionResult<ScrapResultModelResponse>> Fetch([FromBody] FetchRequest fetchRequest)
        {
            CheckModel();

            FetchOffersArguments arguments = _mapper.Map<FetchOffersArguments>(fetchRequest);
            ScrapResultModel result = await Task.Run(() => _processingService.FetchOffers(arguments));

            return Ok(_mapper.Map<ScrapResultModelResponse>(result));
        }

        [HttpGet("testFetch")]
        public async Task<ActionResult<ScrapResultModelResponse>> TestFetch(
            ScrapperType scrapperType,
            PageSourceProviderType pageSourceProviderType)
        {
            ScrapResultModel result
                = await Task.Run(() => _processingService.TestFetchOffers(scrapperType, pageSourceProviderType));

            return Ok(_mapper.Map<ScrapResultModelResponse>(result));
        }

        [HttpPost("triggerNow")]
        public async Task<ActionResult> TriggerNow()
        {
            await _schedulingService.StartNow();

            return Ok();
        }
    }
}