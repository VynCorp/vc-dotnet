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
    [JsonPropertyName("enrichedCompanies")] public long EnrichedCompanies { get; set; }
    [JsonPropertyName("companiesWithIndustry")] public long CompaniesWithIndustry { get; set; }
    [JsonPropertyName("companiesWithGeo")] public long CompaniesWithGeo { get; set; }
    [JsonPropertyName("totalPersons")] public long TotalPersons { get; set; }
    [JsonPropertyName("totalChanges")] public long TotalChanges { get; set; }
    [JsonPropertyName("totalSogcPublications")] public long TotalSogcPublications { get; set; }
}

/// <summary>Pipeline run status.</summary>
public class PipelineStatus
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("itemsProcessed")] public int ItemsProcessed { get; set; }
    [JsonPropertyName("lastCompletedAt")] public string? LastCompletedAt { get; set; }
}

/// <summary>Auditor tenure aggregate statistics.</summary>
public class AuditorTenureStats
{
    [JsonPropertyName("totalTracked")] public long TotalTracked { get; set; }
    [JsonPropertyName("currentAuditors")] public long CurrentAuditors { get; set; }
    [JsonPropertyName("tenuresOver10Years")] public long TenuresOver10Years { get; set; }
    [JsonPropertyName("tenuresOver7Years")] public long TenuresOver7Years { get; set; }
    [JsonPropertyName("avgTenureYears")] public double AvgTenureYears { get; set; }
    [JsonPropertyName("longestTenure")] public LongestTenure? LongestTenure { get; set; }
}

/// <summary>The longest auditor tenure.</summary>
public class LongestTenure
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("auditorName")] public string AuditorName { get; set; } = "";
    [JsonPropertyName("tenureYears")] public double TenureYears { get; set; }
}
