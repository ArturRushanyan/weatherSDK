﻿using Newtonsoft.Json;
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

        private readonly int cacheSize = 10;
        private readonly int cacheExpirationTimeInSeconds = 600;
        private readonly HttpClient httpClient;
        private readonly string apiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}";
        private string apiKey;
        private SDKMode mode;

        public SDKMode Mode => mode;

        private WeatherInfoSDK(string apiKey, SDKMode mode = SDKMode.OnDemend)
        {
            httpClient = new HttpClient();
            this.apiKey = apiKey;
            this.mode = mode;
        }

        public async Task<string> GetCityWeather(string city)
        {

            string url = string.Format(apiUrl, city, apiKey);
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    WeatherApiResponse? weatherData = JsonConvert.DeserializeObject<WeatherApiResponse>(jsonResponse);

                    if (weatherData == null)
                    {
                        throw new BaseWeatherSDKException($"Cannot process response data: {jsonResponse}");
                    }

                    return ConvertWeatherResponseToTargetJson(weatherData);

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

        private string ConvertWeatherResponseToTargetJson(WeatherApiResponse weatherApiResponse)
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

            return JsonConvert.SerializeObject(weatherData);
        }
    }
}