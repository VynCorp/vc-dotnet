using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Analytics resource — cantons, auditors, clustering, anomalies, RFM, cohorts, candidates.</summary>
public class AnalyticsResource
{
    private readonly VynCoClient _client;
    internal AnalyticsResource(VynCoClient client) => _client = client;

    /// <summary>Get aggregate analytics broken down by Swiss canton.</summary>
    public Task<List<CantonDistribution>> CantonsAsync(CancellationToken ct = default)
        => _client.RequestListAsync<CantonDistribution>(HttpMethod.Get, "/v1/analytics/cantons", ct);

    /// <summary>Get auditor market share analytics.</summary>
    public Task<List<AuditorMarketShare>> AuditorsAsync(CancellationToken ct = default)
        => _client.RequestListAsync<AuditorMarketShare>(HttpMethod.Get, "/v1/analytics/auditors", ct);

    /// <summary>Run clustering analysis on companies.</summary>
    public Task<ClusterResponse> ClusterAsync(ClusterRequest request, CancellationToken ct = default)
        => _client.RequestAsync<ClusterResponse>(HttpMethod.Post, "/v1/analytics/cluster", request, ct);

    /// <summary>Detect anomalies in company data.</summary>
    public Task<AnomalyResponse> AnomaliesAsync(AnomalyRequest request, CancellationToken ct = default)
        => _client.RequestAsync<AnomalyResponse>(HttpMethod.Post, "/v1/analytics/anomalies", request, ct);

    /// <summary>Get RFM (Recency, Frequency, Monetary) segments.</summary>
    public Task<RfmSegmentsResponse> RfmSegmentsAsync(CancellationToken ct = default)
        => _client.RequestAsync<RfmSegmentsResponse>(HttpMethod.Get, "/v1/analytics/rfm-segments", ct);

    /// <summary>Get cohort analysis.</summary>
    public Task<CohortResponse> CohortsAsync(CohortParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.GroupBy is not null) qs.Add($"groupBy={Uri.EscapeDataString(@params.GroupBy)}");
        if (@params?.Metric is not null) qs.Add($"metric={Uri.EscapeDataString(@params.Metric)}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";

        return _client.RequestAsync<CohortResponse>(HttpMethod.Get, $"/v1/analytics/cohorts{query}", ct);
    }

    /// <summary>Get audit candidates with pagination.</summary>
    public Task<PagedResponse<AuditCandidate>> CandidatesAsync(CandidateParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.SortBy is not null) qs.Add($"sortBy={Uri.EscapeDataString(@params.SortBy)}");
        if (@params?.Canton is not null) qs.Add($"canton={Uri.EscapeDataString(@params.Canton)}");
        if (@params?.Page is not null) qs.Add($"page={@params.Page}");
        if (@params?.PageSize is not null) qs.Add($"pageSize={@params.PageSize}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";

        return _client.RequestAsync<PagedResponse<AuditCandidate>>(HttpMethod.Get, $"/v1/analytics/candidates{query}", ct);
    }
}
