using Energize.API.Services.PowerPlants.Factory;

namespace Energize.UnitTests.PowerPlants;

public class PowerPlantFactoryTests
{
    [Theory]
    [InlineData("windturbine", typeof(WindTurbinePlant))]
    [InlineData("gasfired", typeof(GasFiredPlant))]
    [InlineData("turbojet", typeof(TurboJetPlant))]
    public void Create_ShouldReturnCorrectType(string type, Type expectedType)
    {
        // Arrange
        var factory = new PowerPlantFactory();
            
        // Act
        var powerPlant = factory.Create(type);
            
        // Assert
        powerPlant.Should().BeOfType(expectedType);
    }
        
    [Theory]
    [InlineData("invalidType")]
    [InlineData("random")]
    [InlineData("unrecognized")]
    public void Create_ShouldThrowArgumentExceptionForInvalidType(string type)
    {
        // Arrange
        var factory = new PowerPlantFactory();
            
        // Act
        Action act = () => factory.Create(type);
            
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage($"Invalid type: {type}");
    }
        
    [Fact]
    public void Create_ShouldNotBeCaseSensitive()
    {
        // Arrange
        var factory = new PowerPlantFactory();
            
        // Act
        var lowerCasePlant = factory.Create("windturbine");
        var upperCasePlant = factory.Create("WINDTURBINE");
            
        // Assert
        lowerCasePlant.Should().BeOfType<WindTurbinePlant>();
        upperCasePlant.Should().BeOfType<WindTurbinePlant>();
    }
}