var builder = WebApplication.CreateBuilder(args); // creating a web application.

// Add services to the container. before building and setting up dependency injection
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build(); // Building Application with all of its dependencies.

// pipeline injections, method usage.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run(); // run the web application.
