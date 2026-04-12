using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Query parameters for timeline endpoints (v3.1+).</summary>
public class TimelineParams
{
    public string? Since { get; set; }
    public string? Until { get; set; }
    public string? ChangeType { get; set; }
}

/// <summary>A single event on a company timeline (v3.1+).</summary>
public class TimelineEvent
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("category")] public string Category { get; set; } = "";
    [JsonPropertyName("fieldName")] public string? FieldName { get; set; }
    [JsonPropertyName("oldValue")] public string? OldValue { get; set; }
    [JsonPropertyName("newValue")] public string? NewValue { get; set; }
    [JsonPropertyName("summary")] public string? Summary { get; set; }
    [JsonPropertyName("source")] public string? Source { get; set; }
    [JsonPropertyName("severity")] public string? Severity { get; set; }
    [JsonPropertyName("date")] public string Date { get; set; } = "";
}

/// <summary>Chronological timeline of a company's changes and events (v3.1+).</summary>
public class TimelineResponse
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("events")] public List<TimelineEvent> Events { get; set; } = new();
    [JsonPropertyName("totalEvents")] public long TotalEvents { get; set; }
}

/// <summary>AI-generated narrative summary of a company timeline (v3.1+).</summary>
public class TimelineSummaryResponse
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("summary")] public string Summary { get; set; } = "";
    [JsonPropertyName("eventCount")] public long EventCount { get; set; }
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
}
