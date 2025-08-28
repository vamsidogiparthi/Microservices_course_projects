using BuildingBlocks.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        // Example: services.AddScoped<IOrderService, OrderService>();
        services.AddMediatR(sp =>
        {
            sp.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            sp.AddOpenBehavior(typeof(ValidationBehavior<,>));
            sp.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        return services;
    }

}
