using Domain.Constants;

namespace Infra.Models.Messages;

public class PubSubMessage<TData>
{
    public TData? Data { get; set; }

    public string? DataContentType { get; set; }

    public string? Id { get; set; }

    public string? Source { get; set; }

    public string? SpecVersion { get; set; }

    public DateTime? Time { get; set; }

    public string? Type { get; set; }

    public PubSubMessage()
    {
        DataContentType = PubSubMetadata.DataContentType;
        Id = Guid.NewGuid().ToString();
        Time = DateTime.UtcNow;
    }
}
