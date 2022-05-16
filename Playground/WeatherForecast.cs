using System;

namespace Playground
{
    public class WeatherForecast :IWeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }

        public int ModifyTemperatureC()
        {
             return TemperatureC = -100;
        }
    }
}
