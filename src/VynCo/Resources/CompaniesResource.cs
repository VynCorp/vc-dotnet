using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Companies resource — search, list, get, count, statistics, batch lookup.</summary>
public class CompaniesResource
{
    private readonly VynCoClient _client;
    internal CompaniesResource(VynCoClient client) => _client = client;

    /// <summary>List companies with pagination and optional filtering.</summary>
    public Task<PagedResponse<Company>> ListAsync(ListCompaniesParams? @params = null, CancellationToken ct = default)
    {
        var p = @params ?? new ListCompaniesParams();
        var query = $"?page={p.Page}&pageSize={p.PageSize}";
        if (p.Canton is not null) query += $"&canton={Uri.EscapeDataString(p.Canton)}";
        if (p.Search is not null) query += $"&search={Uri.EscapeDataString(p.Search)}";
        if (p.Status is not null) query += $"&status={Uri.EscapeDataString(p.Status)}";
        if (p.TargetStatus is not null) query += $"&targetStatus={Uri.EscapeDataString(p.TargetStatus)}";
        if (p.SortBy is not null) query += $"&sortBy={Uri.EscapeDataString(p.SortBy)}";
        if (p.SortDesc is not null) query += $"&sortDesc={p.SortDesc.Value.ToString().ToLowerInvariant()}";
        if (p.AuditorCategory is not null) query += $"&auditorCategory={Uri.EscapeDataString(p.AuditorCategory)}";

        return _client.RequestAsync<PagedResponse<Company>>(HttpMethod.Get, $"/v1/companies{query}", ct);
    }

    /// <summary>Get a single company by its Swiss UID (e.g., CHE-123.456.789).</summary>
    public Task<Company> GetAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<Company>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>Get total company count with optional filters.</summary>
    public Task<CompanyCount> CountAsync(string? canton = null, string? status = null, string? auditorCategory = null, CancellationToken ct = default)
    {
        var query = "";
        var sep = "?";
        if (canton is not null) { query += $"{sep}canton={Uri.EscapeDataString(canton)}"; sep = "&"; }
        if (status is not null) { query += $"{sep}status={Uri.EscapeDataString(status)}"; sep = "&"; }
        if (auditorCategory is not null) { query += $"{sep}auditorCategory={Uri.EscapeDataString(auditorCategory)}"; }

        return _client.RequestAsync<CompanyCount>(HttpMethod.Get, $"/v1/companies/count{query}", ct);
    }

    /// <summary>Get aggregate statistics across the registry.</summary>
    public Task<CompanyStatistics> StatisticsAsync(CancellationToken ct = default)
        => _client.RequestAsync<CompanyStatistics>(HttpMethod.Get, "/v1/companies/statistics", ct);

    /// <summary>Full-text search across company name, purpose, and address.</summary>
    public Task<List<Company>> SearchAsync(CompanySearchRequest request, CancellationToken ct = default)
        => _client.RequestListAsync<Company>(HttpMethod.Post, "/v1/companies/search", request, ct);

    /// <summary>Batch lookup up to 500 companies by UIDs.</summary>
    public Task<List<Company>> BatchAsync(BatchLookupRequest request, CancellationToken ct = default)
        => _client.RequestListAsync<Company>(HttpMethod.Post, "/v1/companies/batch", request, ct);
}
