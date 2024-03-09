using System.Net;

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
    }
}
