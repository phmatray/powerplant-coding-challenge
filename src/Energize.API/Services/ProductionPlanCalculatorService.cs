using Grpc.Core;

namespace Energize.API.Services;

public class ProductionPlanCalculatorService : ProductionPlanCalculator.ProductionPlanCalculatorBase
{
    private readonly ILogger<ProductionPlanCalculatorService> _logger;
    private readonly IPowerPlantFactory _powerPlantFactory;
    
    public ProductionPlanCalculatorService(
        ILogger<ProductionPlanCalculatorService> logger,
        IPowerPlantFactory powerPlantFactory)
    {
        _logger = logger;
        _powerPlantFactory = powerPlantFactory;
    }

    /// <summary>
    /// Calculates the production plan based on the provided payload.
    /// </summary>
    /// <param name="request">The payload request containing load, power plants, and fuels.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>The calculated production plan reply.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="RpcException">Thrown when an internal error occurs.</exception>
    public override async Task<PayloadReply> CalculateProductionPlan(PayloadRequest request, ServerCallContext context)
    {
        ValidateRequest(request);
        _logger.LogInformation("Starting calculation for production plan with Load: {Load}", request.Load);

        var productionPlans = CalculateProductionPlans(request);
    
        _logger.LogInformation("Completed calculation for production plan.");
        var reply = new PayloadReply { Powerplants = { productionPlans } };
        return await Task.FromResult(reply);
    }

    private IEnumerable<ProductionPlanReply> CalculateProductionPlans(PayloadRequest request)
    {
        var productionPlans = new List<ProductionPlanReply>();
        var remainingLoad = request.Load;

        // Preprocess PowerPlants to map them with their types.
        var powerPlants = request.Powerplants
            .ToDictionary(p => p, p => _powerPlantFactory.Create(p.Type));

        // Calculate production for wind turbines first as their fuel cost is zero
        foreach (var (powerplant, _) in powerPlants.Where(p => p.Key.Type == "windturbine"))
        {
            var production = Math.Max(0.0, Math.Round(powerplant.Pmax * (request.Fuels.Wind / 100.0), 1));
            productionPlans.Add(new ProductionPlanReply { Name = powerplant.Name, P = production });
            remainingLoad = Math.Max(0, Math.Round(remainingLoad - production, 1));
        }

        // Sort the remaining powerplants by their efficiency and fuel cost
        var sortedPowerPlants = powerPlants
            .Where(p => p.Key.Type != "windturbine")
            .OrderBy(p => CalculateCostPerMwh(p.Key, p.Value, request.Fuels))
            .ToList();

        foreach (var (powerplant, _) in sortedPowerPlants)
        {
            var production = 0.0;
            if (remainingLoad > 0 && powerplant.Pmin <= remainingLoad)
            {
                var availableProduction = Math.Min(remainingLoad, powerplant.Pmax);
                production = Math.Max(powerplant.Pmin, availableProduction);
                production = Math.Round(production, 1);
                remainingLoad = Math.Max(0, Math.Round(remainingLoad - production, 1));
            }

            productionPlans.Add(new ProductionPlanReply { Name = powerplant.Name, P = production });
        }
        
        return productionPlans;
    }

    private static void ValidateRequest(PayloadRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
    }
    
    private static double CalculateCostPerMwh(PowerPlant plant, IPowerPlant powerPlantType, Fuels fuels)
        => powerPlantType.CalculateFuelCost(fuels, plant.Efficiency) + powerPlantType.CalculateCo2Cost(fuels);
    
    
    
    // curl -X POST "http://localhost:8888/productionplan" -H "Content-Type: application/json" -d '{"load": 910,"fuels":{"gas(euro/MWh)": 13.4,"kerosine(euro/MWh)": 50.8,"co2(euro/ton)": 20,"wind(%)": 60},"powerplants": [{"name": "gasfiredbig1","type": "gasfired","efficiency": 0.53,"pmin": 100,"pmax": 460},{"name": "gasfiredbig2","type": "gasfired","efficiency": 0.53,"pmin": 100,"pmax": 460},{"name": "gasfiredsomewhatsmaller","type": "gasfired","efficiency": 0.37,"pmin": 40,    "pmax": 210},{"name": "tj1","type": "turbojet","efficiency": 0.3,"pmin": 0,"pmax": 16},{"name": "windpark1","type": "windturbine","efficiency": 1,"pmin": 0,"pmax": 150},{"name": "windpark2","type": "windturbine","efficiency": 1,"pmin": 0,"pmax": 36}]}'
}