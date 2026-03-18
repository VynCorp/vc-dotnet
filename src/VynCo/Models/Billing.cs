using System.Text.Json.Serialization;

namespace VynCo.Models;

public class SessionUrl
{
    [JsonPropertyName("url")] public string Url { get; set; } = "";
}

public class CheckoutSessionRequest
{
    [JsonPropertyName("tier")] public string? Tier { get; set; }
}
