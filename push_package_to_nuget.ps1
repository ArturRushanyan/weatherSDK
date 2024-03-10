param ([Parameter(Mandatory)]$PackageName, [Parameter(Mandatory)]$NuGetApiKey, [Parameter(Mandatory)]$BuildPath)
$source = "https://api.nuget.org/v3/index.json";

if(-not($PackageName)) { 
    Write-Error You must specify PackageName e.g WeatherSDK.1.0.1.nupkg 
}
if(-not($NuGetApiKey)) { 
    Write-Error You must specify NuGetApiKey. You may generate it on a NuGet 
}
if(-not($BuildPath)) { 
    Write-Error You must specify BuildPath.
}
else {
    Write-Host "Publishing package "$PackageName" to NuGet source "$source""
    dotnet nuget push $BuildPath\$PackageName --api-key $NuGetApiKey --source $source
}
