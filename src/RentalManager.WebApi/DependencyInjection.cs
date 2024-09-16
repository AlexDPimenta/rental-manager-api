using Carter;
using Carter.OpenApi;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RentalManager.WebApi.Events;
using RentalManager.WebApi.Events.Consumers;
using RentalManager.WebApi.Persistence.Context;
using RentalManager.WebApi.Persistence.Repository.DriversRepository;
using RentalManager.WebApi.Persistence.Repository.MotorCycleRepository;
using RentalManager.WebApi.Service;
using RentalManager.WebApi.Settings;
using System.Reflection;
using System.Text.Json.Serialization;

namespace RentalManager.WebApi;

public static class DependencyInjection
{
    private static readonly Assembly _Assembly = typeof(Program).Assembly;
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProblemDetails();
        services.AddValidatorsFromAssembly(_Assembly);
        services.AddCarter();
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
        services.AddMediator();

        services.AddMassTransitConfiguration(configuration);

        services.AddSwagger();

        services.AddAzureStorage(configuration);

        services.AddPersistence(configuration);


        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();        

        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "RentalManager WebApi", Version = "v1" });
            opt.DocInclusionPredicate((docName, apiDesc) =>
            apiDesc.ActionDescriptor.EndpointMetadata.OfType<IIncludeOpenApi>().Any());

        });

        return services;
    }

    public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var messageSettings = configuration.GetSection(MessageBrokerSettings.SettingsKey)
            .Get<MessageBrokerSettings>();

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
            x.SetKebabCaseEndpointNameFormatter();

            x.AddRider(rider =>
            {
                rider.AddConsumer<MotorCycleCreatedConsumer>(configure
                    => configure.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(30))));               

                rider.AddProducer<MotorCycleCreated>(messageSettings.Topics!.MotorCycleCreated);

                rider.UsingKafka((context, k) =>
                {
                    k.Host(messageSettings!.Host);
                    k.TopicEndpoint<MotorCycleCreated>(messageSettings.Topics!.MotorCycleCreated, nameof(MotorCycleCreated), e => 
                    {
                        e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
                        e.ConfigureConsumer<MotorCycleCreatedConsumer>(context);
                    });

                });
            });
        });

        return services;
    }

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services
            .AddMediatR(config => config.RegisterServicesFromAssembly(_Assembly));            

        return services;
    }

    // add persistence
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RentalManagerDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IMotorCycleRepository, MotorCycleRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();

        return services;
    }
    
    public static IServiceCollection AddAzureStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureStorageSettings>(
            configuration.GetSection("AzureStorageSettings"));

        services.AddSingleton<AzureStorageService>(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<AzureStorageSettings>>();
            return new AzureStorageService(settings);
        });

        services.AddScoped<IAzureStorageService, AzureStorageService>();

        return services;
    }


}
