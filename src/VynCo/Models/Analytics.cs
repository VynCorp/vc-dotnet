using System.Text.Json.Serialization;

namespace VynCo.Models;

public class ClusteringRequest
{
    [JsonPropertyName("k")] public int K { get; set; } = 5;
    [JsonPropertyName("features")] public List<string>? Features { get; set; }
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("limit")] public int Limit { get; set; } = 500;
}

public class AnomalyDetectionRequest
{
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("limit")] public int Limit { get; set; } = 500;
}

/// <summary>Parameters for cohort analytics.</summary>
public class CohortAnalyticsParams
{
    public string? GroupBy { get; set; }
    public string? Canton { get; set; }
}

/// <summary>Parameters for change velocity analytics.</summary>
public class VelocityParams
{
    public int Days { get; set; } = 30;
}
