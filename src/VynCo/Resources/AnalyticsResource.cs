using System.Text.Json;
using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Analytics resource — clustering, anomaly detection, cohort analysis, and segmentation.</summary>
public class AnalyticsResource
{
    private readonly VynCoClient _client;
    internal AnalyticsResource(VynCoClient client) => _client = client;

    /// <summary>Run K-Means clustering on companies. Cost: 25 credits.</summary>
    public Task<JsonElement> ClusterAsync(ClusteringRequest request, CancellationToken ct = default)
        => _client.RequestAsync<JsonElement>(HttpMethod.Post, "/api/v1/analytics/cluster", request, ct);

    /// <summary>Detect statistical anomalies in company data using DBSCAN. Cost: 20 credits.</summary>
    public Task<JsonElement> DetectAnomaliesAsync(AnomalyDetectionRequest? request = null, CancellationToken ct = default)
        => _client.RequestAsync<JsonElement>(HttpMethod.Post, "/api/v1/analytics/anomalies", request, ct);

    /// <summary>Get cohort analytics — companies grouped by founding year, canton, legal form, etc. Cost: 10 credits.</summary>
    public Task<JsonElement> CohortsAsync(CohortAnalyticsParams? @params = null, CancellationToken ct = default)
    {
        var query = "";
        var sep = "?";
        if (@params?.GroupBy is not null) { query += $"{sep}groupBy={Uri.EscapeDataString(@params.GroupBy)}"; sep = "&"; }
        if (@params?.Canton is not null) { query += $"{sep}canton={Uri.EscapeDataString(@params.Canton)}"; }

        return _client.RequestAsync<JsonElement>(HttpMethod.Get, $"/api/v1/analytics/cohorts{query}", ct);
    }

    /// <summary>Get aggregate analytics broken down by Swiss canton. Cost: 3 credits.</summary>
    public Task<JsonElement> CantonsAsync(CancellationToken ct = default)
        => _client.RequestAsync<JsonElement>(HttpMethod.Get, "/api/v1/analytics/cantons", ct);

    /// <summary>Get analytics on auditor categories and distribution. Cost: 3 credits.</summary>
    public Task<JsonElement> AuditorsAsync(CancellationToken ct = default)
        => _client.RequestAsync<JsonElement>(HttpMethod.Get, "/api/v1/analytics/auditors", ct);

    /// <summary>Get RFM (Recency, Frequency, Monetary) segments for companies. Cost: 15 credits.</summary>
    public Task<JsonElement> RfmSegmentsAsync(CancellationToken ct = default)
        => _client.RequestAsync<JsonElement>(HttpMethod.Get, "/api/v1/analytics/rfm-segments", ct);

    /// <summary>Get the rate and volume of commercial register changes over time. Cost: 5 credits.</summary>
    public Task<JsonElement> VelocityAsync(VelocityParams? @params = null, CancellationToken ct = default)
    {
        var days = @params?.Days ?? 30;
        return _client.RequestAsync<JsonElement>(HttpMethod.Get, $"/api/v1/analytics/velocity?days={days}", ct);
    }
}
