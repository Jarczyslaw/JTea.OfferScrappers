using JTea.OfferScrappers.WindowsService.Core.Services;
using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Requests;
using JTea.OfferScrappers.WindowsService.Responses;
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
            OfferHeaderModel result = _offerHeadersService.Create(offerHeader);

            return Ok(_mapper.Map<OfferHeaderModelResponse>(result));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            bool deleted = _offerHeadersService.Delete(id);

            return Ok(deleted);
        }

        [HttpDelete("all")]
        public ActionResult DeleteAll()
        {
            _offerHeadersService.DeleteAll();

            return Ok();
        }

        [HttpGet("all")]
        public ActionResult<GetAllOfferHeadersResponse> GetAll()
        {
            List<OfferHeaderModel> result = _offerHeadersService.GetAll();

            return Ok(new GetAllOfferHeadersResponse
            {
                OfferHeaders = _mapper.Map<List<OfferHeaderModelResponse>>(result)
            });
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
            OfferHeaderModel result = _offerHeadersService.GetById(id);

            return Ok(_mapper.Map<OfferHeaderModelResponse>(result));
        }

        [HttpPut("{id}/enabled")]
        public ActionResult<bool> SetEnabled(int id, bool enabled)
        {
            bool updated = _offerHeadersService.SetEnabled(id, enabled);

            return Ok(updated);
        }

        [HttpPut]
        public ActionResult<OfferHeaderModelResponse> Update([FromBody] UpdateOfferHeaderRequest request)
        {
            CheckModel();

            UpdateOfferHeader update = _mapper.Map<UpdateOfferHeader>(request);
            OfferHeaderModel updated = _offerHeadersService.Update(update);

            return Ok(updated);
        }
    }
}