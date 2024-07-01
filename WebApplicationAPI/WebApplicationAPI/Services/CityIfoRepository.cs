﻿using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.DBContext;
using WebApplicationAPI.Entities;

namespace WebApplicationAPI.Services
{
    public class CityIfoRepository : ICityInfoRepository
    {
        private readonly CItyInfoDBContext _context;
        public CityIfoRepository(CItyInfoDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Id).ToListAsync();
        }
        public async Task<City> GetCityAsync(int id, bool IncludePoints)
        {
            if (IncludePoints)
                return await _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == id).FirstOrDefaultAsync();
            return await _context.Cities.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId)
        {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task<PointOfInterest> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
        {

            return await _context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();

        }

        public async Task<bool> CheckCityExistAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c=>c.Id == cityId);
        }

       public async Task AddPointOfInterestAsync(int cityid, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityid, false);
            if(city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }
        public async Task<bool> SaveChangesAsync()
        {
            return(await _context.SaveChangesAsync()>=0);
        }
    }
}
