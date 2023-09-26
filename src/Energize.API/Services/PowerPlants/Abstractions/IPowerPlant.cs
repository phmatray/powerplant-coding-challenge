namespace Energize.API.Services.PowerPlants.Abstractions;

public interface IPowerPlant
{
    string Type { get; }
    double CalculateFuelCost(Fuels fuels, double efficiency);
    double CalculateCo2Cost(Fuels fuels);
}