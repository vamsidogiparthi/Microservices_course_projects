using Catalog.API.Data;
using Catalog.API.Products.CreateProduct;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container before building the app.

//builder.Services.AddCarter(new DependencyContextAssemblyCatalog(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

builder.Services.AddCarter();
builder.Services.AddMarten(
    options =>
    {        
        var connectionString = builder.Configuration.GetConnectionString("CatalogDb");
        ArgumentException.ThrowIfNullOrEmpty(connectionString);
        options.Connection(connectionString);
        options.DatabaseSchemaName = "catalog_db";          
    }).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql((serviceProvider) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("CatalogDb");
        ArgumentException.ThrowIfNullOrEmpty(connectionString);
        return connectionString;
    }, name: "CatalogDbHealthCheck");

var app = builder.Build();

// After building the app, configure it or use the services.


app.MapCarter();
app.UseExceptionHandler(opt => { });
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
