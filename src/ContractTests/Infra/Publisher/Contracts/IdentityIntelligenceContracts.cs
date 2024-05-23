namespace ContractTests.Infra.Publisher.Contracts;

public class IdentityIntelligenceContracts
{
    private IdentityIntelligenceContracts()
    {
    }

    public static IdentityIntelligenceContracts Post => new();
    public static IdentityIntelligenceContracts Receive => new();

    public IdentityIntelligencePublisherConsumer Consumer = new();
    public IdentityIntelligencePublisherProvider Publisher = new();
}
