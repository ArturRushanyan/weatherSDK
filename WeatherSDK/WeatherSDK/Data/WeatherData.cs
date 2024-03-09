namespace WeatherSDK.Data
{
    public class WeatherData
    {
        public WeatherInfo weather;
        public TemperatureInfo temperature;
        public int visibility;
        public WindInfo wind;
        public long datetime;
        public SysInfo sys;
        public int timezone;
        public string name;
    }

    public class WeatherInfo
    {
        public string main;
        public string description;
    }

    public class TemperatureInfo
    {
        public double temp;
        public double feels_like;
    }

    public class WindInfo
    {
        public double speed;
    }

    public class SysInfo
    {
        public long sunrise;
        public long sunset;
    }
}
