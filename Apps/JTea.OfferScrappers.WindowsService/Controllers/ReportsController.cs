using JTea.OfferScrappers.WindowsService.Controllers.Common.Responses;
using JTea.OfferScrappers.WindowsService.Controllers.Reports.Requests;
using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IReportsService _reportsService;

        public ReportsController(
            IMapper mapper,
            IReportsService reportsService)
        {
            _mapper = mapper;
            _reportsService = reportsService;
        }

        [HttpGet("all")]
        public ActionResult<List<OfferHeaderModelResponse>> GetAll()
        {
            List<OfferHeaderModel> result = _reportsService.GetAll();

            return Ok(_mapper.Map<List<OfferHeaderModelResponse>>(result));
        }

        [HttpPost("filter")]
        public ActionResult<List<OfferHeaderModelResponse>> GetByFilter(OfferHeadersFilterRequest request)
        {
            OfferHeadersFilter filter = _mapper.Map<OfferHeadersFilter>(request);
            List<OfferHeaderModel> result = _reportsService.GetByFilter(filter);

            return Ok(_mapper.Map<List<OfferHeaderModelResponse>>(result));
        }

        [HttpGet("{id}")]
        public ActionResult<OfferHeaderModelResponse> GetById(int id)
        {
            Result<OfferHeaderModel> result = _reportsService.GetById(id);

            return CreateActionResult(result, _mapper.Map<OfferHeaderModelResponse>);
        }
    }
}