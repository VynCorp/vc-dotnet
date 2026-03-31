using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A session URL (Stripe checkout or portal).</summary>
public class SessionUrl
{
    [JsonPropertyName("url")] public string Url { get; set; } = "";
}

/// <summary>Request body for creating a checkout session.</summary>
public class CheckoutRequest
{
    [JsonPropertyName("tier")] public string Tier { get; set; } = "";
}
