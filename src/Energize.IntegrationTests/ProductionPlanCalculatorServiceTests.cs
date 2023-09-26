using Energize.API.Services.PowerPlants.Implementations;
using Energize.IntegrationTests.Helpers;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Energize.IntegrationTests;

public class ProductionPlanCalculatorServiceTests
{
    private readonly Mock<ILogger<ProductionPlanCalculatorService>> _mockLogger = new();
    private readonly Mock<IPowerPlantFactory> _mockFactory = new();

    [Fact]
    public async Task CalculateProductionPlan_ShouldReturnExpectedResult()
    {
        // Arrange
        var service = new ProductionPlanCalculatorService(_mockLogger.Object, _mockFactory.Object);
        var payloadRequest = new PayloadRequest();
        var context = new Mock<ServerCallContext>().Object;
            
        // Act
        var result = await service.CalculateProductionPlan(payloadRequest, context);
            
        // Assert
        result.Should().NotBeNull();
    }
        
    [Fact]
    public void CalculateProductionPlan_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var service = new ProductionPlanCalculatorService(_mockLogger.Object, _mockFactory.Object);
        var context = new Mock<ServerCallContext>().Object;
            
        // Act
        Func<Task> act = async () => await service.CalculateProductionPlan(null, context);
            
        // Assert
        act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'request')");
    }

    [Fact]
    public async Task CalculateProductionPlan_ShouldReturnExpectedResult_ForGivenInput()
    {
        // Arrange
        var service = new ProductionPlanCalculatorService(_mockLogger.Object, _mockFactory.Object);

        
        var fuels = PayloadRequestFactory.CreateFuels(13.4, 50.8, 20, 60);
        var powerPlants = new List<PowerPlant>
        {
            PayloadRequestFactory.CreatePowerPlant("gasfiredbig1", "gasfired", 0.53, 100, 460),
            PayloadRequestFactory.CreatePowerPlant("gasfiredbig2", "gasfired", 0.53, 100, 460),
            PayloadRequestFactory.CreatePowerPlant("gasfiredsomewhatsmaller", "gasfired", 0.37, 40, 210),
            PayloadRequestFactory.CreatePowerPlant("tj1", "turbojet", 0.3, 0, 16),
            PayloadRequestFactory.CreatePowerPlant("windpark1", "windturbine", 1, 0, 150),
            PayloadRequestFactory.CreatePowerPlant("windpark2", "windturbine", 1, 0, 36),
        };
        
        var payloadRequest = PayloadRequestFactory.CreatePayloadRequest(910, fuels, powerPlants);
        
        var context = new Mock<ServerCallContext>().Object;

        // Setup the mocks
        _mockFactory.Setup(f => f.Create("gasfired")).Returns(new GasFiredPlant());
        _mockFactory.Setup(f => f.Create("turbojet")).Returns(new TurboJetPlant());
        _mockFactory.Setup(f => f.Create("windturbine")).Returns(new WindTurbinePlant());

        // Expected production plans
        var expectedPlans = new List<ProductionPlanReply>
        {
            new() { Name = "windpark1", P = 90.0 },
            new() { Name = "windpark2", P = 21.6 },
            new() { Name = "gasfiredbig1", P = 460.0 },
            new() { Name = "gasfiredbig2", P = 338.4 },
            new() { Name = "gasfiredsomewhatsmaller", P = 0.0 },
            new() { Name = "tj1", P = 0.0 }
        };

        // Act
        var result = await service.CalculateProductionPlan(payloadRequest, context);

        // Assert
        result.Powerplants.Should().BeEquivalentTo(expectedPlans, options => options.WithStrictOrdering());
    }
}