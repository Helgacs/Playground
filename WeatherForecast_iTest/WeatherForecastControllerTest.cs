using System.Collections.Generic;
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
        private ServiceDescriptor _serviceDescriptor;

        [SetUp]
        public void Setup()
        {
            weatherForecast = new Mock<IWeatherForecast>();
            weatherForecast.Setup(x => x.TemperatureC);
            weatherForecast.Setup(x => x.Date);
            weatherForecast.Setup(x => x.Summary);
            weatherForecast.Setup(x => x.TemperatureF);
            weatherForecast.Setup(x => x.ModifyTemperatureC());
            _serviceDescriptor = new ServiceDescriptor(typeof(IWeatherForecast), weatherForecast.Object);
            weatherForecast = new Mock<IWeatherForecast>();
            _testServer = new ConfigurableServer(x => x.Replace(_serviceDescriptor));
            _client = _testServer.CreateClient();
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
            var response = await _client.GetAsync("/WeatherForecast");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Get_5_WeatherForecast_Test()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/weatherforecast");
            HttpResponseMessage response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            WeatherForecast[] forecasts = JsonConvert.DeserializeObject<WeatherForecast[]>(responseContent);
            Assert.AreEqual(5, forecasts.Length);
        }

        [Test]
        public async Task Get_TemperatureC_WeatherForecast_Test()
        {
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