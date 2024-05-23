namespace Infra.Models.Messages;

public abstract class Message
{
    public string Id { get; set; }

    protected Message()
    {
        Id = Guid.NewGuid().ToString();
    }
}
