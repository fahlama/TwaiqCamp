using AutoMapper;
using WebApplicationAPI.Entities;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointOfInterestDTO>();
            CreateMap<City, CityDTO>();
            CreateMap<PointOfInterest, PointOfInterestDTO>();
            CreateMap<PointOfInterestForCreationDTO, PointOfInterest>();
        }
    }
}
