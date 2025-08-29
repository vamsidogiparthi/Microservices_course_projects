

using BuildingBlocks.Exceptions.Handler;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Register API services here
        // Example: services.AddControllers();
        // Example: services.AddSwaggerGen();

        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }


    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline for API services
        // Example: app.UseSwagger();
        // Example: app.UseAuthorization();
        // Example: app.MapControllers();

        app.MapCarter();
        app.UseExceptionHandler(opt => { });
        return app;
    }    
}
