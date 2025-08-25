using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    // Configure the HTTP request pipeline for development environment
    app.UseDeveloperExceptionPage();
    await app.InitialDatabaseAsync();
}

// configure the HTTP request pipeline.
app.Run();
