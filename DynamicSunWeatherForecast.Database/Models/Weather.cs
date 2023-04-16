using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DynamicSunWeatherForecast.Database.Models
{
    public class Weather
    {
        [Key]
        public long DataRowId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? MoscowTime { get; set; }

        public double? AirTemperature { get; set; }

        public double? RelativeAirHumidity { get; set; }

        public double? Td { get; set; }

        public double? AtmospherePressure { get; set; }

        public string? WindFollow { get; set; }

        public double? WindSpeed { get; set; }

        public double? Cloudy { get; set; }

        public double? H { get; set; }

        public double? VV { get; set; }

        public string? WeatherConditions { get; set; }
    }
}
