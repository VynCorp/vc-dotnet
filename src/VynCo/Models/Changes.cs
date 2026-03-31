using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A detected change to a company record.</summary>
public class CompanyChange
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string? CompanyName { get; set; }
    [JsonPropertyName("changeType")] public string ChangeType { get; set; } = "";
    [JsonPropertyName("fieldName")] public string? FieldName { get; set; }
    [JsonPropertyName("oldValue")] public string? OldValue { get; set; }
    [JsonPropertyName("newValue")] public string? NewValue { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("source")] public string? Source { get; set; }
    [JsonPropertyName("detectedAt")] public string DetectedAt { get; set; } = "";
}

public class ChangeStatistics
{
    [JsonPropertyName("totalChanges")] public long TotalChanges { get; set; }
    [JsonPropertyName("changesThisWeek")] public long ChangesThisWeek { get; set; }
    [JsonPropertyName("changesThisMonth")] public long ChangesThisMonth { get; set; }
    [JsonPropertyName("byType")] public JsonElement ByType { get; set; }
}

/// <summary>Query parameters for listing changes.</summary>
public class ChangeListParams
{
    public string? ChangeType { get; set; }
    public string? Since { get; set; }
    public string? Until { get; set; }
    public string? CompanySearch { get; set; }
    public long? Page { get; set; }
    public long? PageSize { get; set; }
}
