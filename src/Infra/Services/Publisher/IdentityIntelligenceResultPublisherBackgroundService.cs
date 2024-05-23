using Infra.Config.PubSub;
using Infra.Models.Messages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Questrade.Library.PubSubClientHelper.Primitives;
using Questrade.Library.PubSubClientHelper.Publisher.Outbox;

namespace Infra.Services.Publisher;

public class IdentityIntelligenceResultPublisherBackgroundService : PubsubPublisherBackgroundService<IdentityIntelligenceResultPublisherConfiguration, PubSubMessage<IdentityIntelligenceResultMessage>>
{
    public IdentityIntelligenceResultPublisherBackgroundService(
        ILoggerFactory logger,
        IMemoryCache memoryCache,
        IdentityIntelligenceResultPublisherConfiguration publisherConfiguration,
        IMessageOutboxStore<PubSubMessage<IdentityIntelligenceResultMessage>> messageOutboxStore,
        IPublisherService<PubSubMessage<IdentityIntelligenceResultMessage>> publisherService,
        IDefaultJsonSerializerOptionsProvider defaultJsonSerializerOptionsProvider,
        IServiceProvider serviceProvider,
        IHostApplicationLifetime hostApplicationLifetime
    )
        : base(
            logger,
            memoryCache,
            publisherConfiguration,
            messageOutboxStore,
            publisherService,
            defaultJsonSerializerOptionsProvider,
            serviceProvider,
            hostApplicationLifetime)
    {
    }
}
