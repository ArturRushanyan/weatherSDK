namespace WeatherSDK.Data
{
    public class CachedWheatherData
    {
        public DateTime LastUpdatedDate;
        public WeatherData Data;

        public CachedWheatherData(DateTime lastUpdatedDate, WeatherData data)
        {
            LastUpdatedDate = lastUpdatedDate;
            Data = data;
        }

        public bool IsValid(int expirationTimeInSeconds)
        {
            TimeSpan diff = DateTime.UtcNow - LastUpdatedDate;

            if (diff.TotalSeconds > expirationTimeInSeconds)
            {
                return false;
            }

            return true;
        }
    }
}
