using System.Text.Json.Serialization;

namespace VynCo.Models;

public class Webhook
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("url")] public string Url { get; set; } = "";
    [JsonPropertyName("events")] public List<string> Events { get; set; } = new();
    [JsonPropertyName("status")] public string Status { get; set; } = "active";
    [JsonPropertyName("secret")] public string? Secret { get; set; }
    [JsonPropertyName("createdAt")] public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("updatedAt")] public DateTime? UpdatedAt { get; set; }
}

public class WebhookCreated
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("url")] public string Url { get; set; } = "";
    [JsonPropertyName("events")] public List<string> Events { get; set; } = new();
    [JsonPropertyName("secret")] public string Secret { get; set; } = "";
    [JsonPropertyName("createdAt")] public DateTime? CreatedAt { get; set; }
}

public class CreateWebhookRequest
{
    [JsonPropertyName("url")] public string Url { get; set; } = "";
    [JsonPropertyName("events")] public List<string> Events { get; set; } = new();
}
