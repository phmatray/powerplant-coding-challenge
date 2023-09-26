namespace Energize.IntegrationTests.Helpers;

public class PayloadRequestFactory
{
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
