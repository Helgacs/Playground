using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Playground;
using Playground.Controllers;

namespace WeatherForecast_iTest
{
    [TestFixture]
    public class Tests
    {
        private HttpClient _client;
        private TestServer _testServer;
        private Mock<IWeatherForecast> weatherForecast;
        private List<IWeatherForecast> weatherForecasts;

        [SetUp]
        public void Setup()
        {
            weatherForecasts = new List<IWeatherForecast>();
            weatherForecast = new Mock<IWeatherForecast>();

        }
        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _testServer?.Dispose();
        }

        [Test]
        public async Task Response_Is_Ok_WeatherForecast_Test()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureServices(x => x.AddSingleton(weatherForecast.Object)));
            _client = _testServer.CreateClient();
            weatherForecast.Setup(x => x.TemperatureC).Returns(30);
            weatherForecast.Setup(x => x.Date).Returns(DateTime.Now);
            weatherForecast.Setup(x => x.Summary).Returns("Cool");
            weatherForecast.Setup(x => x.TemperatureF).Returns(100);
            var response = await _client.GetAsync("/WeatherForecast");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Get_5_WeatherForecast_Test()
        {

            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureServices(x => x.AddSingleton(weatherForecast.Object)));
            _client = _testServer.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/weatherforecast");
            HttpResponseMessage response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            WeatherForecast[] forecasts = JsonConvert.DeserializeObject<WeatherForecast[]>(responseContent);
            Assert.AreEqual(weatherForecasts.Count, forecasts.Length);
            Assert.That(forecasts[0].TemperatureC, Is.EqualTo(30));
        }

        [Test]
        public async Task Get_TemperatureC_WeatherForecast_Test()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureServices(x => x.AddSingleton(weatherForecast.Object)));
            _client = _testServer.CreateClient();
            weatherForecast.Setup(x => x.TemperatureC).Returns(35);
            var request = new HttpRequestMessage(HttpMethod.Get, "/weatherforecast");
            HttpResponseMessage response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            WeatherForecast[] forecasts = JsonConvert.DeserializeObject<WeatherForecast[]>(responseContent);
            List<int> temps = new List<int>();
            foreach (var item in forecasts)
            {
                temps.Add(item.TemperatureC);
            }

            for (int i = 0; i < temps.Count; i++)
            {
                Assert.That(temps[i] >= -20 && temps[i] < 55);
            }
        }


        [Test]
        public async Task TemperatureC_Is_Minus100_WeatherForecast_Test()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureServices(x => x.AddSingleton(weatherForecast.Object)));
            _client = _testServer.CreateClient();
            weatherForecast.Setup(x => x.ModifyTemperatureC());
            var request = new HttpRequestMessage(HttpMethod.Get, "/weatherforecast");
            HttpResponseMessage response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            WeatherForecast[] forecasts = JsonConvert.DeserializeObject<WeatherForecast[]>(responseContent);
            List<int> temps = new List<int>();
            foreach (var item in forecasts)
            {
                item.ModifyTemperatureC();
                temps.Add(item.TemperatureC);
            }

            for (int i = 0; i < temps.Count; i++)
            {
                Assert.That(temps[i].Equals(-100));
            }
        }

        [Test]
        public async Task TemperatureC_Is_Not_Minus100_WeatherForecast_Test()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureServices(x => x.AddSingleton(weatherForecast.Object)));
            _client = _testServer.CreateClient();
            weatherForecast.Setup(x => x.TemperatureC).Returns(30);
            weatherForecast.Setup(x => x.Date).Returns(DateTime.Now);
            weatherForecast.Setup(x => x.Summary).Returns("Cool");
            weatherForecast.Setup(x => x.TemperatureF).Returns(100);
            var request = new HttpRequestMessage(HttpMethod.Get, "/weatherforecast");
            HttpResponseMessage response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            WeatherForecast[] forecasts = JsonConvert.DeserializeObject<WeatherForecast[]>(responseContent);
            List<int> temps = new List<int>();
            foreach (var item in forecasts)
            {
                temps.Add(item.TemperatureC);
            }

            for (int i = 0; i < temps.Count; i++)
            {
                Assert.That(temps[i].Equals(-100), Is.False);
            }
        }
    }
}