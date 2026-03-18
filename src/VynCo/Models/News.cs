using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

public class RecentNewsResponse
{
    [JsonPropertyName("count")] public int Count { get; set; }
    [JsonPropertyName("items")] public List<JsonElement> Items { get; set; } = new();
}
