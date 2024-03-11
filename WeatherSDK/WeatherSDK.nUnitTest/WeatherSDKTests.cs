using Moq;
using Newtonsoft.Json;
using WeatherSDK.Data;
using WeatherSDK.Exceptions;

namespace WeatherSDK.nUnitTest
{
    public class WeatherSDKTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateInstanceFaile() {
            Assert.Throws<UnauthorizedExcpetion>(() => WeatherInfoSDK.CreateInstance("", WeatherInfoSDK.SDKMode.OnDemend));
        }

        [Test]
        public void CreateInstanceSuccess()
        {
            var instance = WeatherInfoSDK.CreateInstance("testapikey", WeatherInfoSDK.SDKMode.OnDemend);
            Assert.IsInstanceOf<WeatherInfoSDK>(instance);
        }

        [Test]
        public void GetCityWeather_Success()
        {
            var expectedWeatherResponseData = new WeatherApiResponse
            {
                coord = new Coord { lon = "-0.1257", lat = "51.5085" },
                weather = new List<Weather> { new Weather { id = 721, description = "haze", main = "Haze", icon = "50d" } },
                baseProperty = "stations",
                main = new Main { temp = 280.38f, feels_like = 278.28f, temp_min = 279.64f, temp_max = 281.13f, pressure = 1003f, humidity = 92f },
                visibility = 3500,
                wind = new Wind { deg = 330, speed = 3.09f },
                clouds = new Clouds { all = 100 },
                dt = 1710141372,
                sys = new Sys { type = 2, id = 2075535, country = "GB", sunrise = 1710138181, sunset = 1710179882 },
                timezone = 0,
                id = 2643743,
                name = "London",
                cod = 200
            };

            var expectedResultData = new WeatherData
            {
                weather = new WeatherInfo { main = "Haze", description = "haze" },
                temperature = new TemperatureInfo { temp = 280.38, feels_like = 278.28 },
                visibility = 3500,
                wind = new WindInfo { speed = 3.09 },
                datetime = 1710141372,
                sys = new SysInfo { sunrise = 1710138181, sunset = 1710179882 },
                timezone = 0,
                name = "London"
            };

            var expectedRresultValueAsString = JsonConvert.SerializeObject(expectedResultData);

            var expectedWeatherResponseDataAsString = JsonConvert.SerializeObject(expectedWeatherResponseData);

            Mock<WeatherInfoSDK> instance = new Mock<WeatherInfoSDK>();
            instance.CallBase = true;
            instance.Setup(x => x.FetchWeatherDataAsync(It.IsAny<HttpClient>(), It.IsAny<string>())).ReturnsAsync(expectedWeatherResponseData);

            var result = instance.Object.GetCityWeather("London");

            Assert.That(result.Result, Is.EqualTo(expectedRresultValueAsString));
        }
    }
}