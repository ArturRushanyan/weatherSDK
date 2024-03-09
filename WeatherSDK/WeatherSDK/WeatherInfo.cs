using Newtonsoft.Json;
using WeatherSDK.Data;

namespace WeatherSDK
{
    public class WeatherInfo
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

        private WeatherInfo(string apiKey, SDKMode mode = SDKMode.OnDemend)
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

                    return jsonResponse;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                
            }
            return "";
        }
    }
}
