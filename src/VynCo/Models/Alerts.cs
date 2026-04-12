using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A saved alert that triggers on matching query results (v3.1+).</summary>
public class Alert
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("queryParams")] public JsonElement QueryParams { get; set; }
    [JsonPropertyName("webhookUrl")] public string? WebhookUrl { get; set; }
    [JsonPropertyName("frequency")] public string Frequency { get; set; } = "";
    [JsonPropertyName("isActive")] public bool IsActive { get; set; }
    [JsonPropertyName("savedSearchId")] public string? SavedSearchId { get; set; }
    [JsonPropertyName("lastTriggeredAt")] public string? LastTriggeredAt { get; set; }
    [JsonPropertyName("lastResultCount")] public int? LastResultCount { get; set; }
    [JsonPropertyName("triggerCount")] public int TriggerCount { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("updatedAt")] public string UpdatedAt { get; set; } = "";
}

/// <summary>Request body for creating an alert (v3.1+).</summary>
public class CreateAlertRequest
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("queryParams")] public object QueryParams { get; set; } = new { };
    [JsonPropertyName("webhookUrl")] public string? WebhookUrl { get; set; }
    /// <summary><c>hourly</c> | <c>daily</c> | <c>weekly</c>. Defaults to <c>daily</c> on the server.</summary>
    [JsonPropertyName("frequency")] public string? Frequency { get; set; }
    [JsonPropertyName("savedSearchId")] public string? SavedSearchId { get; set; }
}
