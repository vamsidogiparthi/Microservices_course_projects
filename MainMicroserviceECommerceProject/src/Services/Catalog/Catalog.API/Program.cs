using FluentValidation;
using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container before building the app.

builder.Services.AddCarter(new DependencyContextAssemblyCatalog(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, ServiceLifetime.Scoped, type => type.InterfaceType.IsClass == true &&  !type.ValidatorType.IsGenericTypeDefinition);

builder.Services.AddMarten(
    options =>
    {        
        var connectionString = builder.Configuration.GetConnectionString("CatalogDb");
        ArgumentException.ThrowIfNullOrEmpty(connectionString);
        options.Connection(connectionString);
        options.DatabaseSchemaName = "catalog_db";          
    }).UseLightweightSessions();

var app = builder.Build();

// After building the app, configure it or use the services.


app.MapCarter();
app.Run();
