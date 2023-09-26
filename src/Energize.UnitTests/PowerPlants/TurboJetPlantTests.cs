namespace Energize.UnitTests.PowerPlants;

public class TurboJetPlantTests
{
    [Fact]
    public void CalculateFuelCost_ShouldReturnCorrectValue()
    {
        // Arrange
        var turboJetPlant = new TurboJetPlant();
        var fuels = new Fuels { Kerosine = 100 };
        const double efficiency = 0.5;
            
        // Act
        var result = turboJetPlant.CalculateFuelCost(fuels, efficiency);
            
        // Assert
        var expected = 200; // 100 * (1 / 0.5)
        result.Should().Be(expected);
    }
        
    [Fact]
    public void CalculateCo2Cost_ShouldReturnZero()
    {
        // Arrange
        var turboJetPlant = new TurboJetPlant();
        var fuels = new Fuels();
            
        // Act
        var result = turboJetPlant.CalculateCo2Cost(fuels);
            
        // Assert
        result.Should().Be(0.0);
    }
        
    [Fact]
    public void Type_ShouldReturnTurboJet()
    {
        // Arrange
        var turboJetPlant = new TurboJetPlant();
            
        // Act
        var type = turboJetPlant.Type;
            
        // Assert
        type.Should().Be("turbojet");
    }
}