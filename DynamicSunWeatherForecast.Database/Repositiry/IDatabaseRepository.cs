using DynamicSunWeatherForecast.Database.Models;

namespace DynamicSunWeatherForecast.Database.Repositiry
{
    public interface IDatabaseRepository
    {
        IReadOnlyList<Weather> GetAllWeather();
        IReadOnlyList<Weather> GetWeather(Mounth mounth, int year);
        Task SaveWeatherAsync(IEnumerable<Weather> dataRows);
    }
}
