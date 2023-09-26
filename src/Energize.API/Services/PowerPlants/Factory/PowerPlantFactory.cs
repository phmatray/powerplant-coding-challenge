using Energize.API.Services.PowerPlants.Implementations;

namespace Energize.API.Services.PowerPlants.Factory;

public class PowerPlantFactory : IPowerPlantFactory
{
    public IPowerPlant Create(string type)
    {
        return type.ToLower() switch
        {
            "windturbine" => new WindTurbinePlant(),
            "gasfired" => new GasFiredPlant(),
            "turbojet" => new TurboJetPlant(),
            _ => throw new ArgumentException($"Invalid type: {type}")
        };
    }
}