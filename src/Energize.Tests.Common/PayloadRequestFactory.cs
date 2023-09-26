using Energize.API;

namespace Energize.Tests.Common;

public class PayloadRequestFactory
{
    public static PayloadRequest CreatePayloadRequest3()
    {
        var fuels = CreateFuels(13.4, 50.8, 20, 60);
        var powerPlants = new List<PowerPlant>
        {
            CreatePowerPlant("gasfiredbig1", "gasfired", 0.53, 100, 460),
            CreatePowerPlant("gasfiredbig2", "gasfired", 0.53, 100, 460),
            CreatePowerPlant("gasfiredsomewhatsmaller", "gasfired", 0.37, 40, 210),
            CreatePowerPlant("tj1", "turbojet", 0.3, 0, 16),
            CreatePowerPlant("windpark1", "windturbine", 1, 0, 150),
            CreatePowerPlant("windpark2", "windturbine", 1, 0, 36),
        };

        return CreatePayloadRequest(910, fuels, powerPlants);
    }
    
    public static PayloadRequest CreatePayloadRequest(int load, Fuels fuels, List<PowerPlant> powerPlants)
    {
        var payloadRequest = new PayloadRequest
        {
            Load = load,
            Fuels = fuels,
        };
        
        payloadRequest.Powerplants.AddRange(powerPlants);
        
        return payloadRequest;
    }

    public static Fuels CreateFuels(double gas, double kerosine, double co2, double wind)
    {
        return new Fuels
        {
            Gas = gas,
            Kerosine = kerosine,
            Co2 = co2,
            Wind = wind
        };
    }

    public static PowerPlant CreatePowerPlant(string name, string type, double efficiency, int pmin, int pmax)
    {
        return new PowerPlant
        {
            Name = name,
            Type = type,
            Efficiency = efficiency,
            Pmin = pmin,
            Pmax = pmax
        };
    }
}
