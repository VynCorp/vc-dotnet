using System.Text.Json.Serialization;

namespace VynCo.Models;

// -- Batch screening (v3.1+) ---------------------------------------------------

/// <summary>Batch screening request body (up to 100 UIDs per call).</summary>
public class BatchScreeningRequest
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
}

/// <summary>A summary of a single screening hit within a batch result (v3.1+).</summary>
public class BatchScreeningHitSummary
{
    [JsonPropertyName("source")] public string Source { get; set; } = "";
    [JsonPropertyName("matchedName")] public string MatchedName { get; set; } = "";
    [JsonPropertyName("score")] public double Score { get; set; }
}

/// <summary>Screening result for a single company in a batch request (v3.1+).</summary>
public class BatchScreeningResultByUid
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("riskLevel")] public string RiskLevel { get; set; } = "";
    [JsonPropertyName("totalHits")] public int TotalHits { get; set; }
    [JsonPropertyName("sourcesChecked")] public List<string> SourcesChecked { get; set; } = new();
    [JsonPropertyName("hits")] public List<BatchScreeningHitSummary> Hits { get; set; } = new();
}

/// <summary>Response from a batch screening request (v3.1+).</summary>
public class BatchScreeningResponse
{
    [JsonPropertyName("results")] public List<BatchScreeningResultByUid> Results { get; set; } = new();
}

// -- Batch risk score (v3.1+) -------------------------------------------------

/// <summary>Batch risk-score request body (up to 50 UIDs per call).</summary>
public class BatchRiskScoreRequest
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
}

/// <summary>A summary risk score for a single company in a batch request (v3.1+).</summary>
public class RiskScoreResult
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("overallScore")] public int OverallScore { get; set; }
    [JsonPropertyName("riskLevel")] public string RiskLevel { get; set; } = "";
}

/// <summary>Response from a batch risk scoring request (v3.1+).</summary>
public class BatchRiskScoreResponse
{
    [JsonPropertyName("results")] public List<RiskScoreResult> Results { get; set; } = new();
}
