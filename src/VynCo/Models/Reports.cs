using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Summary of an industry with company count.</summary>
public class IndustrySummary
{
    [JsonPropertyName("industry")] public string Industry { get; set; } = "";
    [JsonPropertyName("companyCount")] public int CompanyCount { get; set; }
}

/// <summary>List of available industries.</summary>
public class IndustryListResponse
{
    [JsonPropertyName("industries")] public List<IndustrySummary> Industries { get; set; } = new();
    [JsonPropertyName("total")] public int Total { get; set; }
}

/// <summary>A company entry within an industry report.</summary>
public class IndustryCompanyEntry
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("shareCapital")] public double? ShareCapital { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
}

/// <summary>Canton distribution entry.</summary>
public class ReportCantonCount
{
    [JsonPropertyName("canton")] public string Canton { get; set; } = "";
    [JsonPropertyName("count")] public int Count { get; set; }
}

/// <summary>Auditor concentration entry.</summary>
public class ReportAuditorCount
{
    [JsonPropertyName("auditorName")] public string AuditorName { get; set; } = "";
    [JsonPropertyName("count")] public int Count { get; set; }
}

/// <summary>Status distribution entry.</summary>
public class StatusCount
{
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("count")] public int Count { get; set; }
}

/// <summary>Detailed industry report with analytics.</summary>
public class IndustryReportResponse
{
    [JsonPropertyName("industry")] public string Industry { get; set; } = "";
    [JsonPropertyName("companyCount")] public int CompanyCount { get; set; }
    [JsonPropertyName("avgCapital")] public double? AvgCapital { get; set; }
    [JsonPropertyName("medianCapital")] public double? MedianCapital { get; set; }
    [JsonPropertyName("topCompanies")] public List<IndustryCompanyEntry> TopCompanies { get; set; } = new();
    [JsonPropertyName("cantonDistribution")] public List<ReportCantonCount> CantonDistribution { get; set; } = new();
    [JsonPropertyName("recentChanges")] public int RecentChanges { get; set; }
    [JsonPropertyName("auditorConcentration")] public List<ReportAuditorCount> AuditorConcentration { get; set; } = new();
    [JsonPropertyName("statusDistribution")] public List<StatusCount> StatusDistribution { get; set; } = new();
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
}

/// <summary>AI-generated industry narrative report.</summary>
public class GeneratedIndustryReport
{
    [JsonPropertyName("industry")] public string Industry { get; set; } = "";
    [JsonPropertyName("report")] public string Report { get; set; } = "";
    [JsonPropertyName("sources")] public List<string> Sources { get; set; } = new();
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
}
