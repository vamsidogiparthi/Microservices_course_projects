using Basket.API.Basket.StoreBasket;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Marten;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container before building the app.

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblyContaining<Program>(); 
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

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

builder.Services.AddValidatorsFromAssemblyContaining<StoreBasketCommandValidator>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddCarter();
var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(opt => { });

// use the injected services
app.Run();