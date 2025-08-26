using Ordering.Application.Data;
using Ordering.Infrastructure.Data.Interceptors;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure services here
        // Example: services.AddScoped<IOrderRepository, OrderRepository>();
        var connectionString = configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Database connection string is not configured.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, options) => {            
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString); });



        return services;
    }
}
