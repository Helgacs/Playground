using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Playground;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WeatherForecast_iTest
{
    public class ApiWebAppicationFactory: WebApplicationFactory<Startup>
    {
        public static IOptions<WeatherForecast> WeatherConfig()
        {
            return Options.Create<WeatherForecast>(new WeatherForecast());
        }
    }
}
