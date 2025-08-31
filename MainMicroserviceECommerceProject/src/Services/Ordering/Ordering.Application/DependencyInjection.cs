

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register application services here
        // Example: services.AddScoped<IOrderService, OrderService>();
        services.AddMediatR(sp =>
        {
            sp.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            sp.AddOpenBehavior(typeof(ValidationBehavior<,>));
            sp.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddFeatureManagement();
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
        return services;
    }

}
