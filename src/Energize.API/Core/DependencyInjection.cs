using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Energize.API.Core;

internal static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IPowerPlantFactory, PowerPlantFactory>();
    }

    public static void AddGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc().AddJsonTranscoding();
    }

    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddGrpcSwagger();
        services.AddSwaggerGen(SetupSwaggerGen);
        return;

        void SetupSwaggerGen(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Energize.API", Version = "v1" });
        }
    }
}