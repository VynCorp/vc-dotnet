using System.Text.Json.Serialization;

namespace VynCo.Models;

public class Change
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string? CompanyName { get; set; }
    [JsonPropertyName("changeType")] public string ChangeType { get; set; } = "";
    [JsonPropertyName("fieldName")] public string? FieldName { get; set; }
    [JsonPropertyName("oldValue")] public string? OldValue { get; set; }
    [JsonPropertyName("newValue")] public string? NewValue { get; set; }
    [JsonPropertyName("sogcId")] public string? SogcId { get; set; }
    [JsonPropertyName("detectedAt")] public DateTime DetectedAt { get; set; }
    [JsonPropertyName("isReviewed")] public bool IsReviewed { get; set; }
    [JsonPropertyName("reviewedBy")] public string? ReviewedBy { get; set; }
    [JsonPropertyName("isFlagged")] public bool IsFlagged { get; set; }
}

public class ChangeStatistics
{
    [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
    [JsonPropertyName("reviewedCount")] public int ReviewedCount { get; set; }
    [JsonPropertyName("flaggedCount")] public int FlaggedCount { get; set; }
    [JsonPropertyName("changeTypeCounts")] public Dictionary<string, int> ChangeTypeCounts { get; set; } = new();
}

public class ReviewChangeRequest
{
    [JsonPropertyName("reviewNotes")] public string? ReviewNotes { get; set; }
}

public class ReviewResult
{
    [JsonPropertyName("reviewed")] public bool Reviewed { get; set; }
    [JsonPropertyName("changeId")] public Guid ChangeId { get; set; }
}

/// <summary>Parameters for listing changes with pagination.</summary>
public class ListChangesParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public string? CompanyUid { get; set; }
}
