# WeatherSDK

## Introduction

The WeatherSDK provides a simple and flexible way to retrieve weather information for a given city from the OpenWeatherMap API. It supports caching of weather data and has two modes of operation: ``OnDemend`` and ``Polling``.

## Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Exceptions](#exceptions)
- [Upload new version of SDK](#upload__new__version__of__SDK)

## Features
1. In ``OnDemend`` mode, the SDK updates weather information only on customer requests (defalut operation mode). In ``Polling`` mode, the SDK requests new weather information for all stored locations.

2. The WeatherSDK Stores weather information about requested cities and returns the stored value if it is still relevant (considered up to date if less than 10 minutes have passed).

3. Creating two copies of an instance with the same ``API_KEY`` is not possible. The system will return the original instance.

4. SKD caches information for a maximum of 10 cities at a time.

5. Methods throw exceptions with a description of the reason in case of failure.

6. Provides a method to delete an instance.

<!--- 7. Includes unit tests for SDK methods using mocks for network requests. -->



## Installation

``WeatherSDK`` package is available in [NuGet](https://www.nuget.org/packages/WeatherSDK).
 
 The SDK use .Net8 (LTS) Framework


### Usage

1. Create C# project with .Net8 Framework
1. Install ``WeatherSDK`` from [NuGet](https://www.nuget.org/packages/WeatherSDK) package manager.
2. Create instance of SDK by ``CreateInstance("YOUR_API_KEY", OnDemend/Polling)`` method and pass parameters.
3. To get weather of the city call ``GetCityWeather(CityName)`` method. 


### Example

```csharp
    using WeatherSDK;

    try
    {
        WeatherInfoSDK instance = WeatherInfoSDK.CreateInstance("YOUR_API_KEY", WeatherInfoSDK.SDKMode.Polling);
        string weather = await instance.GetCityWeather("CityName");

        // By default the SDK works on-demand mode
        WeatherInfoSDK secondInstance = WeatherInfoSDK.CreateInstance("YOUR_API_KEY_2");
        string weather = await instance.GetCityWeather("CityName");
    } 
    catch (UnauthorizedAccessException ex) 
    {
        // Handle unauthorized exception
    }
    catch (NotFoundExcpetion ex)
    {
        // Handle not fount exception
    }
    catch (Exception ex)
    {
        // Handle exception
    }
```

## Exceptions

The Weather API SDK handles exceptions to provide meaningful error messages and help developers troubleshoot issues. Here's an overview of the exceptions that may be thrown by the SDK and how to handle them:


### 1. Empty or Invalid API Key 

- **Exception Type:** `UnauthorizedExcpetion`
- **Description:** Throws when trying to create an SDK instance with an empty or invalid API key.
- **Handling:** Ensure that you provide a valid API key when creating instance of the SDK.

```csharp
try
{
    // Empty API key
    var weatherApiSdk = WeatherApiSdk.GetInstance("");
}
catch (UnauthorizedExcpetion ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### 2. Not Found

- **Exception Type:** `NotFoundExcpetion`
- **Description:** Throws when the requested city is not found in the OpenWeatherMap database.
- **Handling:** Catch this exception and handle it appropriately based on your application's logic.

```csharp
try
{
    var weatherData = await weatherApiSdk.GetWeatherDataAsync("NonexistentCity");
}
catch (NotFoundExcpetion ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```


### 3. Request Failure

- **Exception Type:** `BaseWeatherSDKException`
- **Description:** Throws when there is a failure in making the API request, such as network issues or server errors.
- **Handling:** Wrap API calls in a try-catch block and catch this exception to handle request failures.

Example:
```csharp
try
{
    var weatherData = await weatherApiSdk.GetCityWeather("CityName");
    // Process weatherData
}
catch (BaseWeatherSDKException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Upload new version of SDK

To upload a new version of a package, please run the ".\push_package_to_nuget.ps1" PowerShell script on a Windows operating system. This script requires the following parameters:
1. Package name
2. NuGet ``API-KEY``
3. Path to the relevant package build version.





