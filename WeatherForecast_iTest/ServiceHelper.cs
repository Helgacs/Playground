using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Moq;
using Playground;

namespace WeatherForecast_iTest
{
    public class ConfigurableStartup : Startup
    {
        private readonly Action<IServiceCollection> configureAction;

        public ConfigurableStartup(IConfiguration configuration, Action<IServiceCollection> configureAction)
            : base(configuration) => this.configureAction = configureAction;

        protected override void ConfigureAdditionalServices(IServiceCollection services)
        {
            configureAction(services);
        }
    }

    public class ConfigurableServer : TestServer
    {
        public static Mock<IWeatherForecast> weatherForecast;
        public ConfigurableServer(Action<IServiceCollection> configureAction = null) : base(CreateBuilder(configureAction))
        {
        }

        private static IWebHostBuilder CreateBuilder(Action<IServiceCollection> configureAction)
        {
            weatherForecast = new Mock<IWeatherForecast>();
            weatherForecast.Setup(x => x.TemperatureC).Returns(30);
            weatherForecast.Setup(x => x.Date).Returns(DateTime.Now);
            weatherForecast.Setup(x => x.Summary).Returns("Cool");
            weatherForecast.Setup(x => x.TemperatureF).Returns(100);
            weatherForecast.Setup(x => x.ModifyTemperatureC());
          
            if (configureAction == null)
            {
                configureAction = (sc) => { };
            }
            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddSingleton(weatherForecast.Object))
                .UseStartup<ConfigurableStartup>();
            return builder;
        }
    }
}
