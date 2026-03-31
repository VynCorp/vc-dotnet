using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Admin dashboard response.</summary>
public class DashboardResponse
{
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
    [JsonPropertyName("data")] public DataCompleteness Data { get; set; } = new();
    [JsonPropertyName("pipelines")] public List<PipelineStatus> Pipelines { get; set; } = new();
    [JsonPropertyName("auditorTenures")] public AuditorTenureStats AuditorTenures { get; set; } = new();
}

/// <summary>Data completeness metrics.</summary>
public class DataCompleteness
{
    [JsonPropertyName("totalCompanies")] public long TotalCompanies { get; set; }
    [JsonPropertyName("withCanton")] public long WithCanton { get; set; }
    [JsonPropertyName("withStatus")] public long WithStatus { get; set; }
    [JsonPropertyName("withLegalForm")] public long WithLegalForm { get; set; }
    [JsonPropertyName("withCapital")] public long WithCapital { get; set; }
    [JsonPropertyName("withIndustry")] public long WithIndustry { get; set; }
    [JsonPropertyName("withAuditor")] public long WithAuditor { get; set; }
    [JsonPropertyName("completenessPct")] public double CompletenessPct { get; set; }
}

/// <summary>Pipeline run status.</summary>
public class PipelineStatus
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("lastRun")] public string? LastRun { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("recordsProcessed")] public long? RecordsProcessed { get; set; }
    [JsonPropertyName("durationSeconds")] public double? DurationSeconds { get; set; }
}

/// <summary>Auditor tenure aggregate statistics.</summary>
public class AuditorTenureStats
{
    [JsonPropertyName("totalTenures")] public long TotalTenures { get; set; }
    [JsonPropertyName("longTenures7plus")] public long LongTenures7Plus { get; set; }
    [JsonPropertyName("avgTenureYears")] public double AvgTenureYears { get; set; }
    [JsonPropertyName("maxTenureYears")] public double MaxTenureYears { get; set; }
}
