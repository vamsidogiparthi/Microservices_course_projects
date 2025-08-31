

using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register API services here
        var dbconnectionString = configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Database connection string is not configured.");
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks()
            .AddSqlServer(dbconnectionString);

        return services;
    }


    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline for API services

        app.MapCarter();
        app.UseExceptionHandler(opt => { });
        app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }    
}
