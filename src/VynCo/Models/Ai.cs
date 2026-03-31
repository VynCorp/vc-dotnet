using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Request body for generating an AI dossier.</summary>
public class AiDossierRequest
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("depth")] public string? Depth { get; set; }
}

/// <summary>AI dossier response.</summary>
public class AiDossierResponse
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("dossier")] public string Dossier { get; set; } = "";
    [JsonPropertyName("sources")] public List<string> Sources { get; set; } = new();
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
}

/// <summary>Request body for AI-powered search.</summary>
public class AiSearchRequest
{
    [JsonPropertyName("query")] public string Query { get; set; } = "";
}

/// <summary>AI search response.</summary>
public class AiSearchResponse
{
    [JsonPropertyName("query")] public string Query { get; set; } = "";
    [JsonPropertyName("explanation")] public string Explanation { get; set; } = "";
    [JsonPropertyName("filtersApplied")] public JsonElement FiltersApplied { get; set; }
    [JsonPropertyName("results")] public List<Company> Results { get; set; } = new();
    [JsonPropertyName("total")] public long Total { get; set; }
}

/// <summary>Request body for risk score assessment.</summary>
public class RiskScoreRequest
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
}

/// <summary>Risk score response.</summary>
public class RiskScoreResponse
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("overallScore")] public int OverallScore { get; set; }
    [JsonPropertyName("riskLevel")] public string RiskLevel { get; set; } = "";
    [JsonPropertyName("breakdown")] public List<RiskFactor> Breakdown { get; set; } = new();
    [JsonPropertyName("assessedAt")] public string AssessedAt { get; set; } = "";
}

/// <summary>A single risk factor in a risk score breakdown.</summary>
public class RiskFactor
{
    [JsonPropertyName("factor")] public string Factor { get; set; } = "";
    [JsonPropertyName("score")] public int Score { get; set; }
    [JsonPropertyName("weight")] public double Weight { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = "";
}
