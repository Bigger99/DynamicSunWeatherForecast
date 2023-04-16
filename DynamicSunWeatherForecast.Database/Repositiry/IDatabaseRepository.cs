using DynamicSunWeatherForecast.Database.Models;

namespace DynamicSunWeatherForecast.Database.Repositiry
{
    public interface IDatabaseRepository
    {
        List<Weather> GetAllWeather();
        List<Weather> GetWeather(Mounth mounth, int year);
        Task SaveWeatherAsync(IEnumerable<Weather> dataRows);
    }
}
