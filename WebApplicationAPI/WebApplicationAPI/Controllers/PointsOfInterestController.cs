using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Entities;
using WebApplicationAPI.Models;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers
{
    [Route("api/cities/{cityId}/pointofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public PointsOfInterestController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDTO>>> GetPointsOfInterestAsync(int cityId)
        {
            if (!await _cityInfoRepository.CheckCityExistAsync(cityId))
                return NotFound();
            var result = await _cityInfoRepository.GetPointsOfInterestAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDTO>>(result));
        }
        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDTO>> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CheckCityExistAsync(cityId))
                return NotFound();
            var PointOfinterest = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (PointOfinterest == null) return NotFound();

            return Ok(_mapper.Map<PointOfInterestDTO>(PointOfinterest));
        }

        [HttpPost]
        public async Task<ActionResult> CreatePointOfInterestAsync(
            int cityId, PointOfInterestForCreationDTO pointofinterest)
        {
            if (!await _cityInfoRepository.CheckCityExistAsync(cityId))
                return NotFound();
            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointofinterest);
           await _cityInfoRepository.AddPointOfInterestAsync(cityId, finalPointOfInterest);
           await _cityInfoRepository.SaveChangesAsync();
            var creationPointOfInterest = _mapper.Map<PointOfInterestDTO>(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = creationPointOfInterest.Id
                }, creationPointOfInterest);
        }
    }
}
