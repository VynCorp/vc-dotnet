using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Companies resource — list, get, count, events, statistics, compare, news, reports, relationships, hierarchy, fingerprint, nearby.</summary>
public class CompaniesResource
{
    private readonly VynCoClient _client;
    internal CompaniesResource(VynCoClient client) => _client = client;

    /// <summary>List companies with pagination and optional filtering.</summary>
    public Task<PagedResponse<Company>> ListAsync(CompanyListParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.Search is not null) qs.Add($"search={Uri.EscapeDataString(@params.Search)}");
        if (@params?.Canton is not null) qs.Add($"canton={Uri.EscapeDataString(@params.Canton)}");
        if (@params?.ChangedSince is not null) qs.Add($"changedSince={Uri.EscapeDataString(@params.ChangedSince)}");
        if (@params?.Page is not null) qs.Add($"page={@params.Page}");
        if (@params?.PageSize is not null) qs.Add($"pageSize={@params.PageSize}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";

        return _client.RequestAsync<PagedResponse<Company>>(HttpMethod.Get, $"/v1/companies{query}", ct);
    }

    /// <summary>Get a single company by its Swiss UID.</summary>
    public Task<Company> GetAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<Company>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>Get total company count.</summary>
    public Task<CompanyCount> CountAsync(CancellationToken ct = default)
        => _client.RequestAsync<CompanyCount>(HttpMethod.Get, "/v1/companies/count", ct);

    /// <summary>Get events for a company.</summary>
    public Task<EventListResponse> EventsAsync(string uid, int? limit = null, CancellationToken ct = default)
    {
        var query = limit.HasValue ? $"?limit={limit.Value}" : "";
        return _client.RequestAsync<EventListResponse>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/events{query}", ct);
    }

    /// <summary>Get aggregate statistics across the registry.</summary>
    public Task<CompanyStatistics> StatisticsAsync(CancellationToken ct = default)
        => _client.RequestAsync<CompanyStatistics>(HttpMethod.Get, "/v1/companies/statistics", ct);

    /// <summary>Compare two or more companies side-by-side.</summary>
    public Task<CompareResponse> CompareAsync(CompareRequest request, CancellationToken ct = default)
        => _client.RequestAsync<CompareResponse>(HttpMethod.Post, "/v1/companies/compare", request, ct);

    /// <summary>Get news for a company.</summary>
    public Task<List<NewsItem>> NewsAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<NewsItem>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/news", ct);

    /// <summary>Get financial reports for a company.</summary>
    public Task<List<CompanyReport>> ReportsAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<CompanyReport>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/reports", ct);

    /// <summary>Get relationships for a company.</summary>
    public Task<List<Relationship>> RelationshipsAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<Relationship>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/relationships", ct);

    /// <summary>Get the corporate hierarchy for a company.</summary>
    public Task<HierarchyResponse> HierarchyAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<HierarchyResponse>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/hierarchy", ct);

    /// <summary>Get the data fingerprint for a company.</summary>
    public Task<Fingerprint> FingerprintAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<Fingerprint>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/fingerprint", ct);

    /// <summary>Find companies near a geographic location.</summary>
    public Task<List<NearbyCompany>> NearbyAsync(NearbyParams @params, CancellationToken ct = default)
    {
        var qs = new List<string> { $"lat={@params.Lat}", $"lng={@params.Lng}" };
        if (@params.RadiusKm.HasValue) qs.Add($"radiusKm={@params.RadiusKm.Value}");
        if (@params.Limit.HasValue) qs.Add($"limit={@params.Limit.Value}");
        var query = "?" + string.Join("&", qs);

        return _client.RequestListAsync<NearbyCompany>(HttpMethod.Get, $"/v1/companies/nearby{query}", ct);
    }
}
