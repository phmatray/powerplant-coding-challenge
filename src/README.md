# Energize

## Abstract

Energize is a .NET-based service designed to optimize power plant energy output based on the provided load, fuel costs, and power plant specifications. It uses sophisticated algorithms to calculate the most cost-effective distribution of load amongst available power plants.

Upgraded for .NET 8.0 in June 2024.

## Prerequisites

-   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Building and Launching the API

### 1. Build the Project

Navigate to the project root directory and run the following command:

```shell
dotnet build
```

### 2. Run the API

Once built, navigate to the directory containing the API project and run:

```shell
dotnet run
```

This will launch the API, and it will be accessible at  or the configured port if changed.

## Tests

### Install Test Dependencies

Run the following command in the folder Energize.E2E to install test dependencies:

```powershell
pwsh bin/Debug/net8.0/playwright.ps1 install
```

### Testing with CURL

To manually test the service with CURL, use the following command, replacing the payload with the actual JSON string if needed.

```shell
curl -X POST "http://localhost:8888/productionplan" -H "Content-Type: application/json" -d '{"Load":910.0,"Fuels":{"Gas":13.4,"Kerosine":50.8,"Co2":20.0,"Wind":60.0},"Powerplants":[{"Name":"gasfiredbig1","Type":"gasfired","Efficiency":0.53,"Pmin":100,"Pmax":460},{"Name":"gasfiredbig2","Type":"gasfired","Efficiency":0.53,"Pmin":100,"Pmax":460},{"Name":"gasfiredsomewhatsmaller","Type":"gasfired","Efficiency":0.37,"Pmin":40,"Pmax":210},{"Name":"tj1","Type":"turbojet","Efficiency":0.3,"Pmin":0,"Pmax":16},{"Name":"windpark1","Type":"windturbine","Efficiency":1.0,"Pmin":0,"Pmax":150},{"Name":"windpark2","Type":"windturbine","Efficiency":1.0,"Pmin":0,"Pmax":36}]}'
```

## Code-analysis

Energize uses Jetbrains Qodana for code-analysis to maintain high code quality and identify potential issues in the codebase.