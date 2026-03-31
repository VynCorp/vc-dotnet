using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A watchlist.</summary>
public class Watchlist
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("description")] public string Description { get; set; } = "";
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("updatedAt")] public string UpdatedAt { get; set; } = "";
}

/// <summary>Watchlist summary (used in list responses).</summary>
public class WatchlistSummary
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("description")] public string Description { get; set; } = "";
    [JsonPropertyName("companyCount")] public long CompanyCount { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}

/// <summary>Request body for creating a watchlist.</summary>
public class CreateWatchlistRequest
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("description")] public string? Description { get; set; }
}

/// <summary>Response containing UIDs of companies in a watchlist.</summary>
public class WatchlistCompaniesResponse
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
}

/// <summary>Request body for adding companies to a watchlist.</summary>
public class AddCompaniesRequest
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
}

/// <summary>Response from adding companies to a watchlist.</summary>
public class AddCompaniesResponse
{
    [JsonPropertyName("added")] public long Added { get; set; }
}
