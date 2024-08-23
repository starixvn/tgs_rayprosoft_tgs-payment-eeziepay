using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tgs.Payment.EeziePay.Client;

public static class EeziePayUtility
{
    public static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        Converters = {
        }
    };
}

