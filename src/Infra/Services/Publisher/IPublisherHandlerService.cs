using Infra.Models.Messages;

namespace Infra.Services.Publisher;

public interface IPublisherHandlerService
{
    public Task Publish(IdentityIntelligenceResultMessage message, CancellationToken cancellationToken);
}
