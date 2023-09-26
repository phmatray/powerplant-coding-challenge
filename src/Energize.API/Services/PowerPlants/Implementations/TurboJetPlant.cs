namespace Energize.API.Services.PowerPlants.Implementations;

public class TurboJetPlant : IPowerPlant
{
    public string Type
        => "turbojet";
    
    public double CalculateFuelCost(Fuels fuels, double efficiency)
        => fuels.Kerosine * (1 / efficiency);
    
    public double CalculateCo2Cost(Fuels fuels)
        => 0.0;
}