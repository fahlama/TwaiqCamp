using WebApplicationAPI.Entities;

namespace WebApplicationAPI.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City> GetCityAsync(int id);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        Task<PointOfInterest> GetPointOfInterestAsync(int cityId,int pointOfInterestId);
    }
}
