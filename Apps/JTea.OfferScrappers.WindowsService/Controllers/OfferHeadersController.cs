using JTea.OfferScrappers.WindowsService.Controllers.OfferHeaders.Requests;
using JTea.OfferScrappers.WindowsService.Controllers.OfferHeaders.Responses;
using JTea.OfferScrappers.WindowsService.Core.Services;
using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JToolbox.Core.Models.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace JTea.OfferScrappers.WindowsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferHeadersController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IOfferHeadersService _offerHeadersService;

        public OfferHeadersController(
            IMapper mapper,
            IOfferHeadersService offerHeadersService)
        {
            _offerHeadersService = offerHeadersService;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<OfferHeaderModelResponse> Create([FromBody] CreateOfferHeaderRequest request)
        {
            CheckModel();

            OfferHeaderModel offerHeader = _mapper.Map<OfferHeaderModel>(request);
            Result<OfferHeaderModel> result = _offerHeadersService.Create(offerHeader);

            return CreateActionResult(result, _mapper.Map<OfferHeaderModelResponse>);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Result<bool> result = _offerHeadersService.Delete(id);

            return CreateActionResult(result);
        }

        [HttpDelete("all")]
        public ActionResult DeleteAll()
        {
            _offerHeadersService.DeleteAll();

            return Ok();
        }

        [HttpGet("all")]
        public ActionResult<List<OfferHeaderModelResponse>> GetAll()
        {
            List<OfferHeaderModel> result = _offerHeadersService.GetAll();

            return Ok(_mapper.Map<List<OfferHeaderModelResponse>>(result));
        }

        [HttpPost("filter")]
        public ActionResult<List<OfferHeaderModelResponse>> GetByFilter([FromBody] OfferHeadersFilter filter)
        {
            List<OfferHeaderModel> result = _offerHeadersService.GetByFilter(filter);

            return Ok(_mapper.Map<List<OfferHeaderModelResponse>>(result));
        }

        [HttpGet("{id}")]
        public ActionResult<OfferHeaderModelResponse> GetById(int id)
        {
            Result<OfferHeaderModel> result = _offerHeadersService.GetById(id);

            return CreateActionResult(result, _mapper.Map<OfferHeaderModelResponse>);
        }

        [HttpPut("{id}/enabled")]
        public ActionResult<bool> SetEnabled(int id, bool enabled)
        {
            Result<bool> updated = _offerHeadersService.SetEnabled(id, enabled);

            return CreateActionResult(updated);
        }

        [HttpPut]
        public ActionResult<OfferHeaderModelResponse> Update([FromBody] UpdateOfferHeaderRequest request)
        {
            CheckModel();

            UpdateOfferHeader update = _mapper.Map<UpdateOfferHeader>(request);
            Result<OfferHeaderModel> updated = _offerHeadersService.Update(update);

            return CreateActionResult(updated, _mapper.Map<OfferHeaderModelResponse>);
        }
    }
}