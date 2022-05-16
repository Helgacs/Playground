using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playground.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private List<IWeatherForecast> _weatherForecast = new List<IWeatherForecast>();

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecast weatherForecast)
        {
            _logger = logger;
            _weatherForecast.Add(weatherForecast);
        }

        [HttpGet]
        public IEnumerable<IWeatherForecast> Get()
        {
            return _weatherForecast;
        }
    }
}
