using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();



if(app.Environment.IsDevelopment())
{
    // Configure the HTTP request pipeline for development environment
    app.UseDeveloperExceptionPage();
    await app.InitialDatabaseAsync();
}

app.UseApiServices();

//app.UseExceptionHandler(opt => { });

// configure the HTTP request pipeline.
app.Run();
