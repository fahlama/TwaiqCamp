using Microsoft.AspNetCore.Http;
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

        public CityController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDTO>>> GetCities()
        {
            var cities = await _cityInfoRepository.GetCitiesAsync();
            var Results = new List<CityDTO>();
            foreach (var city in cities)
            {
                Results.Add(new CityDTO
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                });
            }
            return Ok(Results);
        }
    }
}
