using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Request body for compliance screening.</summary>
public class ScreeningRequest
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("uid")] public string? Uid { get; set; }
    [JsonPropertyName("sources")] public List<string>? Sources { get; set; }
}

/// <summary>Screening result response.</summary>
public class ScreeningResponse
{
    [JsonPropertyName("queryName")] public string QueryName { get; set; } = "";
    [JsonPropertyName("queryUid")] public string? QueryUid { get; set; }
    [JsonPropertyName("screenedAt")] public string ScreenedAt { get; set; } = "";
    [JsonPropertyName("hitCount")] public int HitCount { get; set; }
    [JsonPropertyName("riskLevel")] public string RiskLevel { get; set; } = "";
    [JsonPropertyName("hits")] public List<ScreeningHit> Hits { get; set; } = new();
    [JsonPropertyName("sourcesChecked")] public List<string> SourcesChecked { get; set; } = new();
}

/// <summary>A single screening hit.</summary>
public class ScreeningHit
{
    [JsonPropertyName("source")] public string Source { get; set; } = "";
    [JsonPropertyName("matchedName")] public string MatchedName { get; set; } = "";
    [JsonPropertyName("entityType")] public string EntityType { get; set; } = "";
    [JsonPropertyName("score")] public double Score { get; set; }
    [JsonPropertyName("datasets")] public List<string> Datasets { get; set; } = new();
    [JsonPropertyName("details")] public JsonElement Details { get; set; }
}
