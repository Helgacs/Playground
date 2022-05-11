using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Playground;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace WeatherForecast_iTest
{
    public class WeatherForecastControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {

        [Fact]
        public async Task Get_Weather_forecast_test()
        {
            var application = new ApiWebAppicationFactory();
            var client = application.CreateClient();

            var response = await client.GetAsync("/weatherforecast");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AuthorizedByPassed()
        {
            var application = new ApiWebAppicationFactory();
            var client = application.CreateClient();
            var response = client.GetAsync("/weatherforecast");

            var result = JsonConvert.DeserializeObject<WeatherForecast[]>(response.ToString());
            Assert.Equal(5, result.Length);
        }
    }
}
