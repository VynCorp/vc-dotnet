using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>API health status response.</summary>
public class HealthResponse
{
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("database")] public string Database { get; set; } = "";
    [JsonPropertyName("redis")] public string Redis { get; set; } = "";
    [JsonPropertyName("version")] public string Version { get; set; } = "";
}
