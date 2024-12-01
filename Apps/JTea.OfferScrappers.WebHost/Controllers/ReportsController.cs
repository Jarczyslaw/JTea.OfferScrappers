using JTea.OfferScrappers.Logic.Core.Services.Interfaces;
using JTea.OfferScrappers.Logic.Models.Domain;
using JTea.OfferScrappers.WebHost.Controllers.Common.Responses;
using JTea.OfferScrappers.WebHost.Controllers.Reports.Requests;
using JToolbox.Core.Models.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WebHost.Controllers
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
            OfferHeadersFilterModel filter = _mapper.Map<OfferHeadersFilterModel>(request);
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