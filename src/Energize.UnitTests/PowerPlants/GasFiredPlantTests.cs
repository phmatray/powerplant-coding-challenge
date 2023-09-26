namespace Energize.UnitTests.PowerPlants;

public class GasFiredPlantTests
{
    [Fact]
    public void CalculateFuelCost_ShouldReturnCorrectValue()
    {
        // Arrange
        var gasFiredPlant = new GasFiredPlant();
        var fuels = new Fuels { Gas = 100 };
        const double efficiency = 0.5;

        // Act
        var result = gasFiredPlant.CalculateFuelCost(fuels, efficiency);

        // Assert
        const int expected = 200; // 100 * (1 / 0.5)
        result.Should().Be(expected);
    }
    
    [Fact]
    public void CalculateCo2Cost_ShouldReturnCorrectValue()
    {
        // Arrange
        var gasFiredPlant = new GasFiredPlant();
        var fuels = new Fuels { Co2 = 100 };
            
        // Act
        var result = gasFiredPlant.CalculateCo2Cost(fuels);
            
        // Assert
        const int expected = 30; // 100 * 0.3
        result.Should().Be(expected);
    }
        
    [Fact]
    public void Type_ShouldReturnGasFired()
    {
        // Arrange
        var gasFiredPlant = new GasFiredPlant();
            
        // Act
        var type = gasFiredPlant.Type;
            
        // Assert
        type.Should().Be("gasfired");
    }
}