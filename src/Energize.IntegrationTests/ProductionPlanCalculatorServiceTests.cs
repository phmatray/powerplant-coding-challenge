using Energize.API.Services.PowerPlants.Implementations;
using Energize.Tests.Common;
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
        Func<Task> act = async () => await service.CalculateProductionPlan(null!, context);
            
        // Assert
        act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'request')");
    }

    [Fact]
    public async Task CalculateProductionPlan_ShouldReturnExpectedResult_ForGivenInput()
    {
        // Arrange
        var service = new ProductionPlanCalculatorService(_mockLogger.Object, _mockFactory.Object);
        var payloadRequest = PayloadRequestFactory.CreatePayloadRequest3();
        var context = new Mock<ServerCallContext>().Object;

        // Setup the mocks
        _mockFactory.Setup(f => f.Create("gasfired")).Returns(new GasFiredPlant());
        _mockFactory.Setup(f => f.Create("turbojet")).Returns(new TurboJetPlant());
        _mockFactory.Setup(f => f.Create("windturbine")).Returns(new WindTurbinePlant());

        // Expected production plans
        List<ProductionPlanReply> expectedPlans =
        [
            new ProductionPlanReply { Name = "windpark1", P = 90.0 },
            new ProductionPlanReply { Name = "windpark2", P = 21.6 },
            new ProductionPlanReply { Name = "gasfiredbig1", P = 460.0 },
            new ProductionPlanReply { Name = "gasfiredbig2", P = 338.4 },
            new ProductionPlanReply { Name = "gasfiredsomewhatsmaller", P = 0.0 },
            new ProductionPlanReply { Name = "tj1", P = 0.0 }
        ];

        // Act
        var result = await service.CalculateProductionPlan(payloadRequest, context);

        // Assert
        result.Powerplants.Should().BeEquivalentTo(expectedPlans, options => options.WithStrictOrdering());
    }
}