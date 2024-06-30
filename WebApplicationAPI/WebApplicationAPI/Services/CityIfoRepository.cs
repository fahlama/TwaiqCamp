using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.DBContext;
using WebApplicationAPI.Entities;

namespace WebApplicationAPI.Services
{
    public class CityIfoRepository : ICityInfoRepository
    {
        private readonly  CItyInfoDBContext _context;
        public CityIfoRepository(CItyInfoDBContext context)
        {
            _context=context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c=>c.Id).ToListAsync();
        }
        public Task<City> GetCityAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PointOfInterest> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId)
        {
            throw new NotImplementedException();
        }
    }
}
