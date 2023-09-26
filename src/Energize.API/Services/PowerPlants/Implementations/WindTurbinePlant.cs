namespace Energize.API.Services.PowerPlants.Implementations;

public class WindTurbinePlant : IPowerPlant
{
    public string Type
        => "windturbine";
    
    public double CalculateFuelCost(Fuels fuels, double efficiency)
        => 0.0;
    
    public double CalculateCo2Cost(Fuels fuels)
        => 0.0;
}