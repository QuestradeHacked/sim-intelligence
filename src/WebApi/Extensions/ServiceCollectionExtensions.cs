using Application.Config;
using Domain.Services;
using Domain.Utils;
using FluentValidation;
using Infra.Config.PubSub;
using Infra.Models.Messages;
using Infra.Services;
using Infra.Services.Publisher;
using Infra.Services.Subscriber;
using Infra.Validators;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Questrade.Library.HealthCheck.AspNetCore.Extensions;
using Questrade.Library.PubSubClientHelper.Extensions;
using Questrade.Library.PubSubClientHelper.HealthCheck;
using Serilog;
using StatsdClient;
using WebApi.Config;

namespace WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    internal static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder, IdentityIntelligenceConfiguration identityIntelligenceConfiguration)
    {
        builder.AddQuestradeHealthCheck();
        builder.Host.UseSerilog((context, logConfiguration) => logConfiguration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddControllers();
        builder.Services.AddSubscribers(identityIntelligenceConfiguration);
        builder.Services.AddPublisher(identityIntelligenceConfiguration);
        builder.Services.AddDataDogMetrics(builder.Configuration);
        builder.Services.AddAppServices(identityIntelligenceConfiguration);
        builder.Services.AddMediatR(
            AppDomain.CurrentDomain.Load("WebApi"),
            AppDomain.CurrentDomain.Load("Application"),
            AppDomain.CurrentDomain.Load("Infra")
        );
        builder.Services.AddCorrelationContext();
        builder.Services.AddSingleton<IValidator<IdentityIntelligenceMessage>, IdentityIntelligenceMessageValidator>();

        return builder;
    }

    private static void AddDataDogMetrics(this IServiceCollection services, ConfigurationManager configuration)
    {
        var dDMetricsConfig = configuration.GetSection("DataDog:StatsD").Get<DataDogMetricsConfig>();

        services.AddSingleton<IDogStatsd>(_ =>
        {
            var statsdConfig = new StatsdConfig
            {
                Prefix = dDMetricsConfig.Prefix,
                StatsdServerName = dDMetricsConfig.HostName
            };

            var dogStatsdService = new DogStatsdService();
            dogStatsdService.Configure(statsdConfig);

            return dogStatsdService;
        });
    }

    private static IServiceCollection AddAppServices(this IServiceCollection services, IdentityIntelligenceConfiguration configuration)
    {
        services.AddTransient<IMetricService, MetricService>();
        services.AddTransient<IPublisherHandlerService, PublisherHandlerService>();
        services.TryAddSingleton(configuration.TwilioConfiguration);

        return services;
    }

    private static IServiceCollection AddSubscribers(this IServiceCollection services, IdentityIntelligenceConfiguration configuration)
    {
        if (configuration.IdentityIntelligenceSubscriberConfiguration.Enable)
        {
            services.RegisterSubscriber<
                IdentityIntelligenceSubscriberConfiguration,
                IdentityIntelligenceMessage,
                IdentityIntelligenceSubscriber
            >(configuration.IdentityIntelligenceSubscriberConfiguration);
        }

        return services;
    }

    private static IServiceCollection AddPublisher(this IServiceCollection services, IdentityIntelligenceConfiguration configuration)
    {
        if (configuration.IdentityIntelligenceResultPublisherConfiguration.Enable)
        {
            services.RegisterOutboxPublisherWithInMemoryOutbox<
                IdentityIntelligenceResultPublisherConfiguration,
                PubSubMessage<IdentityIntelligenceResultMessage>,
                IdentityIntelligenceResultPublisherService,
                IdentityIntelligenceResultPublisherBackgroundService,
                PublisherHealthCheck<
                    IdentityIntelligenceResultPublisherConfiguration,
                    PubSubMessage<IdentityIntelligenceResultMessage>
                >
            >(configuration.IdentityIntelligenceResultPublisherConfiguration);
        }

        return services;
    }

    private static IServiceCollection AddCorrelationContext(this IServiceCollection services)
    {
        services.AddScoped<CorrelationContext>();

        return services;
    }
}
