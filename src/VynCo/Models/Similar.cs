using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Query parameters for similar-company lookup (v3.1+).</summary>
public class SimilarParams
{
    public long? Limit { get; set; }
}

/// <summary>A company similar to a given query company (v3.1+).</summary>
public class SimilarCompanyResult
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("industry")] public string? Industry { get; set; }
    [JsonPropertyName("legalForm")] public string? LegalForm { get; set; }
    [JsonPropertyName("shareCapital")] public double? ShareCapital { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("similarityScore")] public int SimilarityScore { get; set; }
    [JsonPropertyName("matchingDimensions")] public List<string> MatchingDimensions { get; set; } = new();
}

/// <summary>Response containing companies similar to a query company (v3.1+).</summary>
public class SimilarCompaniesResponse
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("results")] public List<SimilarCompanyResult> Results { get; set; } = new();
}
