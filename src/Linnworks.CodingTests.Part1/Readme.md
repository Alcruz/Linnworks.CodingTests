# Coding Test 1: Consume Linnworks' API
As part of this project I consumed the Linnworks' api and then forward backs the result to an angularjs frontend.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

To be able to run this project you should add Linnworks User session as an app secret. 

```
dotnet user-secrets set "Linnworks:AuthSession" "{LINNWORKS_SESSION}"
```

Also you should verify your Linnworks URI at the app settings, in my case Linnworks was redirecting my computer to another serve located at my region.

```
{
	"Logging": {
		"LogLevel": {
			"Default": "Warning"
		}
	},
	"AllowedHosts": "*",
	"LinnworksBaseUrl": "https://us.linnworks.net/api/"
}

appsettings.json
```

## Libraries I used for this project were:

- XUnit
- WireMock
- NEWTONSOFT JSON.NET

- BOOTSTRAP
- Angularjs UI Bootstrap
- NgTable for Angularjs
