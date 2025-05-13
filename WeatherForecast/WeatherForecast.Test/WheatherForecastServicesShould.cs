using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Contracts;
using WeatherForecast.Exceptions;
using WeatherForecast.Services;
using Moq;

namespace WeatherForecast.Test
{
    [TestClass]
    public class WheatherForecastServicesShould
    {
        [TestMethod]
        public void ReturnsArgumentNullExceptionIfCityIsNull()
        {
            var cityWeatherService = new CityWeatherService(new RandomWeatherService());
            Assert.ThrowsException<ArgumentNullException>(() => cityWeatherService.GetWeather(null));
        }

        [TestMethod]
        public void ReturnsCityNotFoundExceptionIfCityNotExist()
        {
            var cityWeatherService = new CityWeatherService(new RandomWeatherService());
            Assert.ThrowsException<CityNotFoundException>(() => cityWeatherService.GetWeather("No esta"));
        }

        [TestMethod]
        public void ReturnsWeatherSunnyIfRainProbIsLess20()
        {
            var mock = new Mock<IWeatherService>();
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(5);
            var cityWeatherService = new CityWeatherService(mock.Object);
            Assert.AreEqual(Weather.Sunny, cityWeatherService.GetWeather("Madrid"));
        }

        [TestMethod]
        public void ReturnsWeatherCloudyIfRainProbBetween20And49()
        {
            var mock = new Mock<IWeatherService>();
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(20);
            var cityWeatherService = new CityWeatherService(mock.Object);
            Assert.AreEqual(Weather.Cloudy, cityWeatherService.GetWeather("Madrid"));
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(49);
            cityWeatherService = new CityWeatherService(mock.Object);
            Assert.AreEqual(Weather.Cloudy, cityWeatherService.GetWeather("Madrid"));
        }

        [TestMethod]
        public void ReturnsWeatherRainyIfRainProbBetween50And79()
        {
            var mock = new Mock<IWeatherService>();
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(50);
            var cityWeatherService = new CityWeatherService(mock.Object);
            Assert.AreEqual(Weather.Rainy, cityWeatherService.GetWeather("Madrid"));
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(79);
            cityWeatherService = new CityWeatherService(mock.Object);
            Assert.AreEqual(Weather.Rainy, cityWeatherService.GetWeather("Madrid"));
        }

        [TestMethod]
        public void ReturnsWeatherStormyIfRainProb80OrMore()
        {
            var mock = new Mock<IWeatherService>();
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(80);
            var cityWeatherService = new CityWeatherService(mock.Object);
            Assert.AreEqual(Weather.Stormy, cityWeatherService.GetWeather("Madrid"));
            mock.Setup(cityWeatherService => cityWeatherService.RainProbability()).Returns(85);
            cityWeatherService = new CityWeatherService(mock.Object);
            Assert.AreEqual(Weather.Stormy, cityWeatherService.GetWeather("Madrid"));
        }

        [TestMethod]
        public void ReturnsRainProbabilityNumberBetween0And100()
        {
            var randomWeatherService = new RandomWeatherService();
            int number = randomWeatherService.RainProbability();
            Assert.IsTrue(number>0 && number<100);
        }
    }
}
