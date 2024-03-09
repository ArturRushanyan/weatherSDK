using Newtonsoft.Json;

namespace WeatherSDK.Data
{
    public class WeatherApiResponse
    {
        public Coord coord;
        public List<Weather> weather;
        [JsonProperty("base")]
        public string baseProperty;
        public Main main;
        public int visibility;
        public Wind wind;
        public Clouds clouds;
        public long dt;
        public Sys sys;
        public int timezone;
        public int id;
        public string name;
        public int cod;
    }

    public class Coord
    {
        public string lon;
        public string lat;
    }

    public class Weather
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }

    public class Main
    {
        public float temp;
        public float feels_like;
        public float temp_min;
        public float temp_max;
        public float pressure;
        public float humidity;
    }

    public class Wind
    {
        public float speed;
        public float deg;
    }

    public class Clouds
    {
        public int all;
    }

    public class Sys
    {
        public int id;
        public int type;
        public string country;
        public long sunrise;
        public long sunset;
    }
}
