
using Discount.Grpc.Protos;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container before building the app.
//application services
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblyContaining<Program>(); 
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddCarter();

// data services
builder.Services.AddMarten(
options =>
{
    var connectionString = builder.Configuration.GetConnectionString("BasketDb");
    ArgumentException.ThrowIfNullOrEmpty(connectionString);
    options.Connection(connectionString);
    options.DatabaseSchemaName = "basket_db";
    options.Schema.For<ShoppingCart>()
        .Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CacheBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "BasketRedis";
});


// grpc services

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl") ?? throw new ArgumentNullException("DiscountUrl"));
})
    .ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
})
;

//  cross cutting concerns
builder.Services.AddValidatorsFromAssemblyContaining<StoreBasketCommandValidator>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddRedis((provider) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Redis");
        ArgumentException.ThrowIfNullOrEmpty(connectionString);
        return connectionString;
    }, name: "RedisHealthCheck")
    .AddNpgSql((serviceProvider) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("BasketDb");
        ArgumentException.ThrowIfNullOrEmpty(connectionString);
        return connectionString;
    }, name: "BasketDbHealthCheck");

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(opt => { });
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// use the injected services
app.Run();