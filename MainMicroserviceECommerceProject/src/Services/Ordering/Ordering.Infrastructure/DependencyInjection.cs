

using Ordering.Infrastructure.Data.Interceptors;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure services here
        // Example: services.AddScoped<IOrderRepository, OrderRepository>();
        var connectionString = configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Database connection string is not configured.");

        services.AddDbContext<ApplicationDbContext>((sp, options) => {
            options.AddInterceptors(new AuditableEntityInterceptor());
            options.UseSqlServer(connectionString); });



        return services;
    }
}
