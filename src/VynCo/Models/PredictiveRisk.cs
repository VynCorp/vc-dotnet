using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Request body for predictive risk scoring.</summary>
public class PredictiveRiskRequest
{
    [JsonPropertyName("lookbackDays")] public int? LookbackDays { get; set; }
}

/// <summary>A pre-dissolution risk indicator.</summary>
public class PredictiveRiskIndicator
{
    [JsonPropertyName("signal")] public string Signal { get; set; } = "";
    [JsonPropertyName("triggered")] public bool Triggered { get; set; }
    [JsonPropertyName("weight")] public double Weight { get; set; }
    [JsonPropertyName("contribution")] public double Contribution { get; set; }
    [JsonPropertyName("severity")] public string Severity { get; set; } = "";
    [JsonPropertyName("description")] public string Description { get; set; } = "";
}

/// <summary>Predictive risk scoring response with dissolution probability.</summary>
public class PredictiveRiskResponse
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("dissolutionProbability")] public double DissolutionProbability { get; set; }
    [JsonPropertyName("riskLevel")] public string RiskLevel { get; set; } = "";
    [JsonPropertyName("preDissolutionIndicators")] public List<PredictiveRiskIndicator> PreDissolutionIndicators { get; set; } = new();
    [JsonPropertyName("creditRiskScore")] public int CreditRiskScore { get; set; }
    [JsonPropertyName("recommendation")] public string Recommendation { get; set; } = "";
    [JsonPropertyName("assessedAt")] public string AssessedAt { get; set; } = "";
}
