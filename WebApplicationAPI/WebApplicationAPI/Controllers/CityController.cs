using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Entities;
using WebApplicationAPI.Models;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public CityController(ICityInfoRepository cityInfoRepository,IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDTO>>> GetCities()
        {
            var cities = await _cityInfoRepository.GetCitiesAsync();
            //var Results = new List<CityWithoutPointOfInterestDTO>();
            //foreach (var city in cities)
            //{
            //    Results.Add(new CityWithoutPointOfInterestDTO
            //    {
            //        Id = city.Id,
            //        Name = city.Name,
            //        Description = city.Description
            //    });
            //}
            var results=_mapper.Map<IEnumerable<CityWithoutPointOfInterestDTO >>(cities);
            return Ok(results);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCityAsync(int Id,
            bool IncludePointOfinterest=false)
        {
            var cityToReturn= await _cityInfoRepository.GetCityAsync(Id, IncludePointOfinterest);
            if(cityToReturn == null) { return NotFound(); };
            if(IncludePointOfinterest)
            {
                return  Ok(_mapper.Map<CityDTO>(cityToReturn));
            }
            return Ok(_mapper.Map<CityWithoutPointOfInterestDTO>(cityToReturn));
        }
        
    }
}
