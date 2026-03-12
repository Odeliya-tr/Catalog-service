using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using InventoryService.Entities;
using InventoryService.Clients;
using Play.Common.Repositories;
using Play.Common.Settings;
using Play.Common.MassTransit;
using Polly;
using Polly.Timeout;
using MassTransit;
using InventoryService.Consumers;
using InventoryService.Entities;

namespace InventoryService
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            var mongoClient = new MongoClient($"mongodb://{mongoDbSettings.Host}:{mongoDbSettings.Port}");

            services.AddSingleton(serviceProvider =>
            {
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            services.AddSingleton<IRepository<InventoryItem>>(serviceProvider =>
            {
                var database = serviceProvider.GetRequiredService<IMongoDatabase>();
                return new MongoRepository<InventoryItem>(database, "inventoryitems");
            });

            services.AddSingleton<IRepository<CatalogItem>>(serviceProvider =>
            {
                var database = serviceProvider.GetRequiredService<IMongoDatabase>();
                return new MongoRepository<CatalogItem>(database, "catalogitems");
            });

            services.AddHttpClient<CatalogClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
            })
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(
                3, retryAttempt => TimeSpan.FromSeconds(2)
            ))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1))
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(
                3, TimeSpan.FromSeconds(15)
            ));

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host("rabbitmq");
                });
            });
            services.AddMassTransit(config =>
            {
                config.AddConsumer<CatalogItemCreatedConsumer>();
                config.AddConsumer<CatalogItemUpdatedConsumer>();
                config.AddConsumer<CatalogItemDeletedConsumer>();

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host("rabbitmq");
                    configurator.ReceiveEndpoint("catalog-items-created", endpoint =>
                    {
                        endpoint.ConfigureConsumer<CatalogItemCreatedConsumer>(context);
                    });
                    configurator.ReceiveEndpoint("catalog-items-updated", endpoint =>
                    {
                        endpoint.ConfigureConsumer<CatalogItemUpdatedConsumer>(context);
                    });
                    configurator.ReceiveEndpoint("catalog-items-deleted", endpoint =>
                    {
                        endpoint.ConfigureConsumer<CatalogItemDeletedConsumer>(context);
                    });
                });
            });
            services.AddMassTransitHostedService();

            services.AddControllers(option => option.SuppressAsyncSuffixInActionNames = false);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InventoryService", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}