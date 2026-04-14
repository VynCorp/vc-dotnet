using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Query parameters for browsing sanctions lists.</summary>
public class SanctionsSearchParams
{
    public string? Search { get; set; }
    public string? EntityType { get; set; }
    public string? Program { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

/// <summary>A single sanctions list entry.</summary>
public class SanctionEntry
{
    [JsonPropertyName("secoId")] public string SecoId { get; set; } = "";
    [JsonPropertyName("entityType")] public string EntityType { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("aliases")] public List<string> Aliases { get; set; } = new();
    [JsonPropertyName("nationality")] public string? Nationality { get; set; }
    [JsonPropertyName("dateOfBirth")] public string? DateOfBirth { get; set; }
    [JsonPropertyName("address")] public string? Address { get; set; }
    [JsonPropertyName("program")] public string Program { get; set; } = "";
    [JsonPropertyName("listedSince")] public string? ListedSince { get; set; }
    [JsonPropertyName("sourceUrl")] public string SourceUrl { get; set; } = "";
}

/// <summary>Paginated sanctions browse response.</summary>
public class SanctionsListResponse
{
    [JsonPropertyName("items")] public List<SanctionEntry> Items { get; set; } = new();
    [JsonPropertyName("total")] public int Total { get; set; }
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("pageSize")] public int PageSize { get; set; }
}
