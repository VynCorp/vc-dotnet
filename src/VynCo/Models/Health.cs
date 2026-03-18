using System.Text.Json.Serialization;

namespace VynCo.Models;

public class HealthResponse
{
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("uptime")] public string? Uptime { get; set; }
    [JsonPropertyName("checks")] public List<HealthCheck> Checks { get; set; } = new();
}

public class HealthCheck
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("durationMs")] public int? DurationMs { get; set; }
    [JsonPropertyName("message")] public string? Message { get; set; }
}
