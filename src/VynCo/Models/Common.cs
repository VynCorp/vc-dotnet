using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Paginated response wrapper matching the VynCo API pagination format.</summary>
public class PagedResponse<T>
{
    [JsonPropertyName("items")] public List<T> Items { get; set; } = new();
    [JsonPropertyName("total")] public long Total { get; set; }
    [JsonPropertyName("page")] public long Page { get; set; }
    [JsonPropertyName("pageSize")] public long PageSize { get; set; }
}
