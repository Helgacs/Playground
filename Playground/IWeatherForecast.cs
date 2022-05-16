using System;

namespace Playground
{
    public interface IWeatherForecast
    {
        DateTime Date { get; set; }
        int TemperatureC { get; set; }
        int TemperatureF { get; }
        string Summary { get; set; }
        public int ModifyTemperatureC();
    }
}
