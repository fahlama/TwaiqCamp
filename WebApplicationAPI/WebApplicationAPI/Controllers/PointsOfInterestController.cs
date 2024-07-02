using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly ILogger<PointsOfInterestController> _logger;

        public PointsOfInterestController(ICityInfoRepository cityInfoRepository, IMapper mapper,
            ILogger<PointsOfInterestController> logger)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDTO>>> GetPointsOfInterestAsync(int cityId)
        {
            throw new Exception("sample exception");
            try
            {
                
                if (!await _cityInfoRepository.CheckCityExistAsync(cityId))
                {
                    _logger.LogInformation($"this city Id {cityId} is not exist.");
                    return NotFound();
                }
                var result = await _cityInfoRepository.GetPointsOfInterestAsync(cityId);
                return Ok(_mapper.Map<IEnumerable<PointOfInterestDTO>>(result));

            }
            catch (Exception ex) 
            {
                _logger.LogCritical("sorry a problem happened.", ex);
                return StatusCode(500, "problem happened on the server"); 
            }




        }
        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDTO>> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CheckCityExistAsync(cityId))
            {
                
                return NotFound();
            }
                
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

        [HttpDelete("{pointOfInterestID}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId,
            int pointOfInterestID)
        {
            if(! await _cityInfoRepository.CheckCityExistAsync(cityId))
                return NotFound();
            var PointOfInterestToDelete = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestID);
            if(PointOfInterestToDelete == null)
                return NotFound();
            _cityInfoRepository.DeletePointOfInterest(PointOfInterestToDelete);
            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();

        }

        [HttpPut("{pointOfInterestId}")]

        public async Task<ActionResult> UpdatePointOfInterestAsync(int cityId
            , int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            if(! await _cityInfoRepository.CheckCityExistAsync(cityId))
                return NotFound();
            var PointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(
                cityId, pointOfInterestId);
            if(PointOfInterestEntity == null)
                return NotFound();

            _mapper.Map(pointOfInterest, PointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId,int pointOfInterestId,
            
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
            
        {
            if (!await _cityInfoRepository.CheckCityExistAsync(cityId))
                return NotFound();
            var PointOfInterestEntity = await _cityInfoRepository.
                GetPointOfInterestAsync(
                cityId, pointOfInterestId);
            if (PointOfInterestEntity == null)
                return NotFound();

            var PointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(
                PointOfInterestEntity);

            patchDocument.ApplyTo(PointOfInterestToPatch,ModelState);

            if(! ModelState.IsValid) return BadRequest(ModelState);
           
            _mapper.Map(PointOfInterestToPatch, PointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync(); return NoContent();
        }
    }
}
