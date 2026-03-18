using System.Text.Json.Serialization;

namespace VynCo.Models;

public class CompanyWatch
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("teamId")] public Guid? TeamId { get; set; }
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("channel")] public string Channel { get; set; } = "InApp";
    [JsonPropertyName("webhookUrl")] public string? WebhookUrl { get; set; }
    [JsonPropertyName("watchedChangeTypes")] public List<string> WatchedChangeTypes { get; set; } = new();
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
}

public class AddWatchRequest
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string? CompanyName { get; set; }
    [JsonPropertyName("channel")] public string Channel { get; set; } = "InApp";
    [JsonPropertyName("webhookUrl")] public string? WebhookUrl { get; set; }
    [JsonPropertyName("watchedChangeTypes")] public List<string>? WatchedChangeTypes { get; set; }
}

public class ChangeNotification
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("changeId")] public Guid ChangeId { get; set; }
    [JsonPropertyName("changeType")] public string ChangeType { get; set; } = "";
    [JsonPropertyName("summary")] public string Summary { get; set; } = "";
    [JsonPropertyName("channel")] public string Channel { get; set; } = "";
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("sentAt")] public DateTime? SentAt { get; set; }
}
