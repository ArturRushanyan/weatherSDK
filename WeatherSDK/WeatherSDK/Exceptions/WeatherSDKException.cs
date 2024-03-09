namespace WeatherSDK.Exceptions
{
    public class BaseWeatherSDKException : Exception
    {
        public BaseWeatherSDKException(string message) : base(message) { }
    }

    public class UnauthorizedExcpetion : BaseWeatherSDKException
    {
        public UnauthorizedExcpetion() : base("Invalid API token") { }
    }

    public class NotFoundExcpetion : BaseWeatherSDKException
    {
        public NotFoundExcpetion() : base("Invalid city name") { }
    }
}
