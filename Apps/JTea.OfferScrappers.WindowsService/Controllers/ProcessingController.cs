using JTea.OfferScrappers.WindowsService.Controllers.Common.Responses;
using JTea.OfferScrappers.WindowsService.Controllers.Processing.Requests;
using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;
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

        public ProcessingController(
            IMapper mapper,
            IProcessingService processingService)
        {
            _mapper = mapper;
            _processingService = processingService;
        }

        [HttpGet("processAll")]
        public async Task<ActionResult> ProcessAll()
        {
            Result result = await Task.Run(() => _processingService.ProcessAllOfferHeaders(waitIfCurrentlyProcessing: false));

            return CreateActionResult(result);
        }

        [HttpPost("scrap")]
        public async Task<ActionResult<ScrapResultModelResponse>> ScrapOffers([FromBody] ScrapOffersRequest request)
        {
            CheckModel();

            ScrapOffersArgumentsModel arguments = _mapper.Map<ScrapOffersArgumentsModel>(request);
            ScrapResultModel result = await Task.Run(() => _processingService.ScrapOffers(arguments));

            return Ok(_mapper.Map<ScrapResultModelResponse>(result));
        }

        [HttpGet("scrapTest")]
        public async Task<ActionResult<ScrapResultModelResponse>> ScrapTestOffers(
            ScrapperType scrapperType,
            PageSourceProviderType pageSourceProviderType)
        {
            ScrapResultModel result
                = await Task.Run(() => _processingService.ScrapTestOffers(scrapperType, pageSourceProviderType));

            return Ok(_mapper.Map<ScrapResultModelResponse>(result));
        }
    }
}