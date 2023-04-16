using DynamicSunWeatherForecast.Database.DatabaseContext;
using DynamicSunWeatherForecast.Database.Models;

namespace DynamicSunWeatherForecast.Database.Repositiry
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Weather> GetAllWeather()
        {
            return _dbContext.Weather.ToList();
        }

        public List<Weather> GetWeather(Mounth mounth, int year)
        {
            if (mounth == Mounth.None && year == 0)
            {
                return GetAllWeather();
            }
            else if (mounth != Mounth.None && year != 0)
            {
                return _dbContext.Weather.Where(x => x.Date.Month == (int)mounth && x.Date.Year == year).ToList();
            }
            else if (mounth == Mounth.None && year != 0)
            {
                return _dbContext.Weather.Where(x => x.Date.Year == year).ToList();
            }
            else if (mounth != Mounth.None && year == 0)
            {
                return _dbContext.Weather.Where(x => x.Date.Month == (int)mounth).ToList();
            }

            return GetAllWeather();
        }

        public async Task SaveWeatherAsync(IEnumerable<Weather> weatherList)
        {
            _dbContext.Weather.AddRange(weatherList);
            await _dbContext.SaveChangesAsync();
        }
    }
}
