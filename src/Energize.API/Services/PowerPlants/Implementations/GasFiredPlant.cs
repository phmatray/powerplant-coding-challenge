namespace Energize.API.Services.PowerPlants.Implementations;

public class GasFiredPlant : IPowerPlant
{
    public string Type
        => "gasfired";
    
    public double CalculateFuelCost(Fuels fuels, double efficiency)
        => fuels.Gas * (1 / efficiency);
    
    public double CalculateCo2Cost(Fuels fuels)
        => fuels.Co2 * 0.3;
}