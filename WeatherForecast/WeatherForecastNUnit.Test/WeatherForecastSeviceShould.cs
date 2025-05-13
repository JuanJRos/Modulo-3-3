using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WeatherForecast.Exceptions;
using WeatherForecast.Services;
using Moq;
using WeatherForecast.Contracts;

namespace WeatherForecastNUnit.Test
{
    [TestFixture]
    public class WeatherForecastSeviceShould
    {
        private CityWeatherService _cityWeatherService;

        [OneTimeSetUp]
        public void Setup()
        {
            _cityWeatherService = new CityWeatherService(new RandomWeatherService());
        }

        [Test]
        public void ReturnsArgumentNullExceptionIfCityIsNull()
        {
            var cityWeatherService = new CityWeatherService(new RandomWeatherService());
            Assert.Throws<ArgumentNullException>(() => _cityWeatherService.GetWeather(null));
        }

        [Test]
        public void ReturnsCityNotFoundExceptionIfCityNotExist()
        {
            var cityWeatherService = new CityWeatherService(new RandomWeatherService());
            Assert.Throws<CityNotFoundException>(() => _cityWeatherService.GetWeather("No esta"));
        }

        [Test]
        public void ReturnsWeatherSunnyIfRainProbIsLess20()
        {
            var mock = new Mock<IWeatherService>();
            mock.Setup(_cityWeatherService => _cityWeatherService.RainProbability()).Returns(5);
            var cityWeatherService = new CityWeatherService(mock.Object);
            Assert.That(Weather.Sunny == cityWeatherService.GetWeather("Madrid"));
        }

        [Test]
        public void ReturnsWeatherCloudyIfRainProbBetween20And49()
        {
            var mock = new Mock<IWeatherService>();
            mock.Setup(_cityWeatherService => _cityWeatherService.RainProbability()).Returns(20);
            var cityWeatherService = new CityWeatherService(mock.Object);
            Assert.That(Weather.Cloudy == cityWeatherService.GetWeather("Madrid"));
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(49);
            cityWeatherService = new CityWeatherService(mock.Object);
            Assert.That(Weather.Cloudy == cityWeatherService.GetWeather("Madrid"));
        }

        [Test]
        public void ReturnsWeatherRainyIfRainProbBetween50And79()
        {
            var mock = new Mock<IWeatherService>();
            mock.Setup(_cityWeatherService => _cityWeatherService.RainProbability()).Returns(50);
            var cityWeatherService = new CityWeatherService(mock.Object);
            Assert.That(Weather.Rainy == cityWeatherService.GetWeather("Madrid"));
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(79);
            cityWeatherService = new CityWeatherService(mock.Object);
            Assert.That(Weather.Rainy == cityWeatherService.GetWeather("Madrid"));
        }

        [Test]
        public void ReturnsWeatherStormyIfRainProb80OrMore()
        {
            var mock = new Mock<IWeatherService>();
            mock.Setup(_cityWeatherService => _cityWeatherService.RainProbability()).Returns(80);
            var cityWeatherService = new CityWeatherService(mock.Object);
            Assert.That(Weather.Stormy == cityWeatherService.GetWeather("Madrid"));
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(85);
            cityWeatherService = new CityWeatherService(mock.Object);
            Assert.That(Weather.Stormy == cityWeatherService.GetWeather("Madrid"));
        }

        [Test]
        public void ReturnsRainProbabilityNumberBetween0And100()
        {
            var randomWeatherService = new RandomWeatherService();
            int number = randomWeatherService.RainProbability();
            Assert.IsTrue(number > 0 && number < 100);
        }
    }
}
