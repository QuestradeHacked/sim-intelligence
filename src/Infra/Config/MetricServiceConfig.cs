using Infra.Models;

namespace Infra.Config;

public class MetricServiceConfig
{
    public CollectLevel CollectLevel { get; set; } = CollectLevel.Basic;
}
