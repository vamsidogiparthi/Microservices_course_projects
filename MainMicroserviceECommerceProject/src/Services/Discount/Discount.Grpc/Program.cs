//using Discount.Grpc.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddDbContext<DiscountContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Database"));
});


var app = builder.Build();
app.ApplyDbMigrations();
// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGrpcReflectionService(); // <-- This line enables reflection
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
