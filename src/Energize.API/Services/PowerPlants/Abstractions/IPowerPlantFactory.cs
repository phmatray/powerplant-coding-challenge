namespace Energize.API.Services.PowerPlants.Abstractions;

public interface IPowerPlantFactory
{
    IPowerPlant Create(string type);
}