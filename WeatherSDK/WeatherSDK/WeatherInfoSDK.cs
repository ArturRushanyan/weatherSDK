using Newtonsoft.Json;
using System.Net;
using WeatherSDK.Data;
using WeatherSDK.Exceptions;

namespace WeatherSDK
{
    public class WeatherInfoSDK
    {
        public enum SDKMode
        {
            Polling,
            OnDemend
        }

        private static Task pollingTask;
        private static Dictionary<string, WeatherInfoSDK> instances = new Dictionary<string, WeatherInfoSDK>();

        private readonly int cacheSize = 10;
        private readonly int cacheExpirationTimeInSeconds = 600;
        private readonly HttpClient httpClient;
        private readonly string apiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}";
        private string apiKey;
        private SDKMode mode;
        private Dictionary<string, CachedWheatherData> weatherCachedData;

        public SDKMode Mode => mode;

        private WeatherInfoSDK(string apiKey, SDKMode mode = SDKMode.OnDemend)
        {
            httpClient = new HttpClient();
            this.apiKey = apiKey;
            this.mode = mode;
            weatherCachedData = new Dictionary<string, CachedWheatherData>();
        }

        public static WeatherInfoSDK CreateInstance(string apiKey, SDKMode mode = SDKMode.OnDemend)
        {
            if (!instances.TryGetValue(apiKey, out var instance))
            {
                instance = new WeatherInfoSDK(apiKey, mode);
                instances.Add(apiKey, instance);

                if (mode == SDKMode.Polling && pollingTask == null)
                {
                    RunPollingTask();
                }
            }

            return instance;
        }

        public static void DeleteInstance(WeatherInfoSDK instance)
        {
            if (instances.ContainsKey(instance.apiKey))
            {
                instances.Remove(instance.apiKey);
            }
        }

        public async Task<string> GetCityWeather(string city)
        {
            CachedWheatherData cachedData;

            if (weatherCachedData.TryGetValue(city, out cachedData))
            {
                if (cachedData.IsValid(cacheExpirationTimeInSeconds))
                {
                    return SerializeWeatherData(cachedData.Data);
                }
            }

            string url = string.Format(apiUrl, city, apiKey);

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    WeatherApiResponse? weatherResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(jsonResponse);

                    if (weatherResponse == null)
                    {
                        throw new BaseWeatherSDKException($"Cannot process response data: {jsonResponse}");
                    }

                    WeatherData weatherData = ConvertWeatherResponseToTargetJson(weatherResponse);

                    if (cachedData != null)
                    {
                        cachedData.LastUpdatedDate = DateTime.UtcNow;
                        cachedData.Data = weatherData;
                    }
                    else
                    {
                        if (weatherCachedData.Count >= cacheSize)
                        {
                            weatherCachedData.Remove(weatherCachedData.Keys.Last());
                        }
                        weatherCachedData.Add(city, new CachedWheatherData(DateTime.UtcNow, weatherData));
                    }

                    return SerializeWeatherData(weatherData);
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedExcpetion();
                        case HttpStatusCode.NotFound:
                            throw new NotFoundExcpetion();
                        default:
                            throw new BaseWeatherSDKException($"Exception: {(int)response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BaseWeatherSDKException(ex.Message);
            }
        }

        private WeatherData ConvertWeatherResponseToTargetJson(WeatherApiResponse weatherApiResponse)
        {
            WeatherData weatherData = new WeatherData
            {
                weather = new WeatherInfo
                {
                    main = weatherApiResponse.weather[0].main,
                    description = weatherApiResponse.weather[0].description
                },
                temperature = new TemperatureInfo
                {
                    temp = weatherApiResponse.main.temp,
                    feels_like = weatherApiResponse.main.feels_like
                },
                visibility = weatherApiResponse.visibility,
                wind = new WindInfo
                {
                    speed = weatherApiResponse.wind.speed
                },
                datetime = weatherApiResponse.dt,
                sys = new SysInfo
                {
                    sunrise = weatherApiResponse.sys.sunrise,
                    sunset = weatherApiResponse.sys.sunset
                },
                timezone = weatherApiResponse.timezone,
                name = weatherApiResponse.name
            };

            return weatherData;
        }

        private string SerializeWeatherData(WeatherData weatherData)
        { 
            return JsonConvert.SerializeObject(weatherData);
        }

        private static void RunPollingTask()
        {
            pollingTask = Task.Run(async () =>
            {
                while (true)
                {

                    await Task.Delay(10000);
                    List<WeatherInfoSDK> apiKeys = instances.Values.ToList();

                    foreach (WeatherInfoSDK instance in apiKeys)
                    {
                        if (instance.Mode == SDKMode.Polling)
                        {
                            List<string> cities = instance.weatherCachedData.Keys.ToList();
                            foreach (string city in cities)
                            {
                                await instance.GetCityWeather(city);
                            }
                        }
                    }
                }
            });
        }
    }
}
