namespace Energize.UnitTests.PowerPlants;

public class WindTurbinePlantTests
{
    [Fact]
    public void CalculateFuelCost_ShouldAlwaysReturnZero()
    {
        // Arrange
        var windTurbinePlant = new WindTurbinePlant();
        var fuels = new Fuels();
        const double efficiency = 0.5; // The value won't impact the result
            
        // Act
        var result = windTurbinePlant.CalculateFuelCost(fuels, efficiency);
            
        // Assert
        result.Should().Be(0.0);
    }
        
    [Fact]
    public void CalculateCo2Cost_ShouldAlwaysReturnZero()
    {
        // Arrange
        var windTurbinePlant = new WindTurbinePlant();
        var fuels = new Fuels();
            
        // Act
        var result = windTurbinePlant.CalculateCo2Cost(fuels);
            
        // Assert
        result.Should().Be(0.0);
    }
        
    [Fact]
    public void Type_ShouldReturnWindTurbine()
    {
        // Arrange
        var windTurbinePlant = new WindTurbinePlant();
            
        // Act
        var type = windTurbinePlant.Type;
            
        // Assert
        type.Should().Be("windturbine");
    }
}