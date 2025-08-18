using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

app.UseApiServices();
// configure the HTTP request pipeline.
app.Run();
