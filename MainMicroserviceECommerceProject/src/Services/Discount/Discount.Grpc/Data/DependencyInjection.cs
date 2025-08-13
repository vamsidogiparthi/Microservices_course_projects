namespace Discount.Grpc.Data;

public static class DependencyInjection
{
    public static IApplicationBuilder ApplyDbMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        context.Database.MigrateAsync();

        return app;
    }

}
