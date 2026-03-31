using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A Swiss company record.</summary>
public class Company
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("legalForm")] public string? LegalForm { get; set; }
    [JsonPropertyName("shareCapital")] public double? ShareCapital { get; set; }
    [JsonPropertyName("industry")] public string? Industry { get; set; }
    [JsonPropertyName("auditorCategory")] public string? AuditorCategory { get; set; }
    [JsonPropertyName("updatedAt")] public string? UpdatedAt { get; set; }
}

public class CompanyCount
{
    [JsonPropertyName("count")] public long Count { get; set; }
}

public class CompanyStatistics
{
    [JsonPropertyName("total")] public long Total { get; set; }
    [JsonPropertyName("byStatus")] public Dictionary<string, long> ByStatus { get; set; } = new();
    [JsonPropertyName("byCanton")] public Dictionary<string, long> ByCanton { get; set; } = new();
    [JsonPropertyName("byLegalForm")] public Dictionary<string, long> ByLegalForm { get; set; } = new();
}

/// <summary>Query parameters for listing companies.</summary>
public class CompanyListParams
{
    public string? Search { get; set; }
    public string? Canton { get; set; }
    public string? ChangedSince { get; set; }
    public long? Page { get; set; }
    public long? PageSize { get; set; }
}

/// <summary>Response wrapper for event listing.</summary>
public class EventListResponse
{
    [JsonPropertyName("events")] public List<CompanyEvent> Events { get; set; } = new();
    [JsonPropertyName("count")] public long Count { get; set; }
}

/// <summary>A CloudEvent-style company event.</summary>
public class CompanyEvent
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("ceType")] public string CeType { get; set; } = "";
    [JsonPropertyName("ceSource")] public string CeSource { get; set; } = "";
    [JsonPropertyName("ceTime")] public string CeTime { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("category")] public string Category { get; set; } = "";
    [JsonPropertyName("severity")] public string Severity { get; set; } = "";
    [JsonPropertyName("summary")] public string Summary { get; set; } = "";
    [JsonPropertyName("detailJson")] public JsonElement DetailJson { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}

/// <summary>Request body for comparing companies.</summary>
public class CompareRequest
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
}

/// <summary>Company comparison response.</summary>
public class CompareResponse
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
    [JsonPropertyName("names")] public List<string> Names { get; set; } = new();
    [JsonPropertyName("dimensions")] public List<ComparisonDimension> Dimensions { get; set; } = new();
}

/// <summary>A single dimension in a company comparison.</summary>
public class ComparisonDimension
{
    [JsonPropertyName("field")] public string Field { get; set; } = "";
    [JsonPropertyName("label")] public string Label { get; set; } = "";
    [JsonPropertyName("values")] public List<string?> Values { get; set; } = new();
}

/// <summary>A news item for a company.</summary>
public class NewsItem
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("summary")] public string? Summary { get; set; }
    [JsonPropertyName("source")] public string? Source { get; set; }
    [JsonPropertyName("sourceType")] public string SourceType { get; set; } = "";
    [JsonPropertyName("publishedAt")] public string PublishedAt { get; set; } = "";
    [JsonPropertyName("sourceUrl")] public string? Url { get; set; }
}

/// <summary>A financial report for a company.</summary>
public class CompanyReport
{
    [JsonPropertyName("reportType")] public string ReportType { get; set; } = "";
    [JsonPropertyName("fiscalYear")] public int? FiscalYear { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = "";
    [JsonPropertyName("sourceUrl")] public string? SourceUrl { get; set; }
    [JsonPropertyName("publicationDate")] public string PublicationDate { get; set; } = "";
}

/// <summary>A relationship between two companies.</summary>
public class Relationship
{
    [JsonPropertyName("relatedUid")] public string RelatedUid { get; set; } = "";
    [JsonPropertyName("relatedName")] public string RelatedName { get; set; } = "";
    [JsonPropertyName("relationshipType")] public string RelationshipType { get; set; } = "";
    [JsonPropertyName("sharedPersons")] public List<string> SharedPersons { get; set; } = new();
}

/// <summary>Corporate hierarchy response.</summary>
public class HierarchyResponse
{
    [JsonPropertyName("parent")] public JsonElement? Parent { get; set; }
    [JsonPropertyName("subsidiaries")] public List<JsonElement> Subsidiaries { get; set; } = new();
    [JsonPropertyName("siblings")] public List<JsonElement> Siblings { get; set; } = new();
}

/// <summary>Company data fingerprint.</summary>
public class Fingerprint
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("industrySector")] public string? IndustrySector { get; set; }
    [JsonPropertyName("industryGroup")] public string? IndustryGroup { get; set; }
    [JsonPropertyName("industry")] public string? Industry { get; set; }
    [JsonPropertyName("sizeCategory")] public string? SizeCategory { get; set; }
    [JsonPropertyName("employeeCountEstimate")] public int? EmployeeCountEstimate { get; set; }
    [JsonPropertyName("capitalAmount")] public double? CapitalAmount { get; set; }
    [JsonPropertyName("capitalCurrency")] public string? CapitalCurrency { get; set; }
    [JsonPropertyName("revenue")] public double? Revenue { get; set; }
    [JsonPropertyName("netIncome")] public double? NetIncome { get; set; }
    [JsonPropertyName("auditorTier")] public string? AuditorTier { get; set; }
    [JsonPropertyName("changeFrequency")] public long ChangeFrequency { get; set; }
    [JsonPropertyName("boardSize")] public long BoardSize { get; set; }
    [JsonPropertyName("companyAge")] public long CompanyAge { get; set; }
    [JsonPropertyName("canton")] public string Canton { get; set; } = "";
    [JsonPropertyName("legalForm")] public string LegalForm { get; set; } = "";
    [JsonPropertyName("hasParentCompany")] public bool HasParentCompany { get; set; }
    [JsonPropertyName("subsidiaryCount")] public long SubsidiaryCount { get; set; }
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
    [JsonPropertyName("fingerprintVersion")] public string FingerprintVersion { get; set; } = "";
}

/// <summary>Query parameters for finding nearby companies.</summary>
public class NearbyParams
{
    public double Lat { get; set; }
    public double Lng { get; set; }
    public double? RadiusKm { get; set; }
    public long? Limit { get; set; }
}

/// <summary>A company near a geographic location.</summary>
public class NearbyCompany
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("distance")] public double Distance { get; set; }
    [JsonPropertyName("latitude")] public double Latitude { get; set; }
    [JsonPropertyName("longitude")] public double Longitude { get; set; }
}
