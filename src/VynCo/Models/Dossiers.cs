using System.Text.Json.Serialization;

namespace VynCo.Models;

public class Dossier
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string? CompanyName { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("summary")] public string? Summary { get; set; }
    [JsonPropertyName("riskScore")] public double? RiskScore { get; set; }
    [JsonPropertyName("generatedAt")] public DateTime GeneratedAt { get; set; }
    [JsonPropertyName("executiveSummary")] public string? ExecutiveSummary { get; set; }
    [JsonPropertyName("keyInsights")] public List<string> KeyInsights { get; set; } = new();
    [JsonPropertyName("riskFactors")] public List<string> RiskFactors { get; set; } = new();
    [JsonPropertyName("llmProvider")] public string? LlmProvider { get; set; }
    [JsonPropertyName("llmModel")] public string? LlmModel { get; set; }
    [JsonPropertyName("version")] public int Version { get; set; }
}

public class DossierStatistics
{
    [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
    [JsonPropertyName("completedCount")] public int CompletedCount { get; set; }
    [JsonPropertyName("failedCount")] public int FailedCount { get; set; }
    [JsonPropertyName("pendingCount")] public int PendingCount { get; set; }
}

public class GenerateDossierRequest
{
    [JsonPropertyName("type")] public string Type { get; set; } = "standard";
}
