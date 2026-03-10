using System;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace Play.Common.MassTransit
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host("rabbitmq");
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }
    }
}