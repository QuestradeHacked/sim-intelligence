using System.Text.Json;

namespace ContractTests.Helpers;

public static class ReadHelper
{
    public static string ReadAsString(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}
