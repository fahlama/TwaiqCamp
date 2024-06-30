using WebApplicationAPI.Entities;

namespace WebApplicationAPI.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City> GetCityAsync(int id,bool IncludePoints);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        Task<PointOfInterest> GetPointOfInterestAsync(int cityId,int pointOfInterestId);
    }
}
