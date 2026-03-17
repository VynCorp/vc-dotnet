using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Paginated response wrapper matching the VynCo API pagination format.</summary>
public class PagedResponse<T>
{
    [JsonPropertyName("items")] public List<T> Items { get; set; } = new();
    [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("pageSize")] public int PageSize { get; set; }
    [JsonPropertyName("totalPages")] public int TotalPages { get; set; }
    [JsonPropertyName("hasPreviousPage")] public bool HasPreviousPage { get; set; }
    [JsonPropertyName("hasNextPage")] public bool HasNextPage { get; set; }
}

/// <summary>Request to batch-lookup entities by UIDs.</summary>
public class BatchLookupRequest
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
}
