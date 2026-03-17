using System.Text.Json.Serialization;

namespace VynCo.Models;

public class Company
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("legalForm")] public string? LegalForm { get; set; }
    [JsonPropertyName("legalFormId")] public int? LegalFormId { get; set; }
    [JsonPropertyName("purpose")] public string? Purpose { get; set; }
    [JsonPropertyName("registeredAddress")] public string? RegisteredAddress { get; set; }
    [JsonPropertyName("shareCapital")] public decimal? ShareCapital { get; set; }
    [JsonPropertyName("currency")] public string? Currency { get; set; }
    [JsonPropertyName("currentAuditor")] public string? CurrentAuditor { get; set; }
    [JsonPropertyName("auditorCategory")] public string AuditorCategory { get; set; } = "";
    [JsonPropertyName("isActive")] public bool IsActive { get; set; }
    [JsonPropertyName("foundingDate")] public DateTime? FoundingDate { get; set; }
    [JsonPropertyName("deletionDate")] public DateTime? DeletionDate { get; set; }
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updatedAt")] public DateTime UpdatedAt { get; set; }
}

public class CompanyCount
{
    [JsonPropertyName("count")] public int Count { get; set; }
}

public class CompanyStatistics
{
    [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
    [JsonPropertyName("enrichedCount")] public int EnrichedCount { get; set; }
    [JsonPropertyName("cantonCounts")] public Dictionary<string, int> CantonCounts { get; set; } = new();
    [JsonPropertyName("auditorCategoryCounts")] public Dictionary<string, int> AuditorCategoryCounts { get; set; } = new();
}

public class CompanySearchRequest
{
    [JsonPropertyName("query")] public string Query { get; set; } = "";
    [JsonPropertyName("limit")] public int Limit { get; set; } = 50;
}

/// <summary>Parameters for listing companies with pagination and filtering.</summary>
public class ListCompaniesParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public string? Canton { get; set; }
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? TargetStatus { get; set; }
    public string? SortBy { get; set; }
    public bool? SortDesc { get; set; }
    public string? AuditorCategory { get; set; }
}
