using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A company-role entry within a board overlap.</summary>
public class OverlapCompanyRole
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("role")] public string Role { get; set; } = "";
}

/// <summary>A board member appearing in multiple compared companies.</summary>
public class BoardOverlap
{
    [JsonPropertyName("personName")] public string PersonName { get; set; } = "";
    [JsonPropertyName("companies")] public List<OverlapCompanyRole> Companies { get; set; } = new();
}

/// <summary>An auditor serving one or more compared companies.</summary>
public class CompAuditorEntry
{
    [JsonPropertyName("auditorName")] public string AuditorName { get; set; } = "";
    [JsonPropertyName("companyCount")] public int CompanyCount { get; set; }
    [JsonPropertyName("companyUids")] public List<string> CompanyUids { get; set; } = new();
    [JsonPropertyName("groupShare")] public double GroupShare { get; set; }
}

/// <summary>Auditor analysis across compared companies.</summary>
public class AuditorAnalysis
{
    [JsonPropertyName("auditorDistribution")] public List<CompAuditorEntry> AuditorDistribution { get; set; } = new();
    [JsonPropertyName("uniqueAuditorCount")] public int UniqueAuditorCount { get; set; }
    [JsonPropertyName("concentrationFlag")] public bool ConcentrationFlag { get; set; }
}

/// <summary>A single governance score factor.</summary>
public class GovernanceFactor
{
    [JsonPropertyName("factor")] public string Factor { get; set; } = "";
    [JsonPropertyName("score")] public int Score { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = "";
}

/// <summary>Per-company governance score.</summary>
public class GovernanceScore
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("score")] public int Score { get; set; }
    [JsonPropertyName("factors")] public List<GovernanceFactor> Factors { get; set; } = new();
}

/// <summary>Request body for AI comparative dossier generation.</summary>
public class ComparativeRequest
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
    [JsonPropertyName("focus")] public string? Focus { get; set; }
}

/// <summary>AI-generated comparative dossier for multiple companies.</summary>
public class ComparativeResponse
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
    [JsonPropertyName("focus")] public string Focus { get; set; } = "";
    [JsonPropertyName("report")] public string Report { get; set; } = "";
    [JsonPropertyName("boardOverlaps")] public List<BoardOverlap> BoardOverlaps { get; set; } = new();
    [JsonPropertyName("auditorAnalysis")] public AuditorAnalysis? AuditorAnalysis { get; set; }
    [JsonPropertyName("governanceScores")] public List<GovernanceScore> GovernanceScores { get; set; } = new();
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
}
