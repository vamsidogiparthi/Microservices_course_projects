
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        // register rabittmq

        var rabbitMqSettings = configuration.GetSection("MessageBroker") ?? throw new ArgumentException("MessageBroker config cannot be empty");

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter(); // naming convention for the endpoints

            if (assembly is not null) x.AddConsumers(assembly); // register all consumers from the assembly

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(rabbitMqSettings["Host"]!), h =>
                {
                    h.Username(rabbitMqSettings["UserName"]!);
                    h.Password(rabbitMqSettings["Password"]!);
                });
                configurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}
