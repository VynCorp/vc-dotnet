using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Changes resource — list, get by company, statistics.</summary>
public class ChangesResource
{
    private readonly VynCoClient _client;
    internal ChangesResource(VynCoClient client) => _client = client;

    /// <summary>List recent company changes with pagination and filtering.</summary>
    public Task<PagedResponse<CompanyChange>> ListAsync(ChangeListParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.ChangeType is not null) qs.Add($"changeType={Uri.EscapeDataString(@params.ChangeType)}");
        if (@params?.Since is not null) qs.Add($"since={Uri.EscapeDataString(@params.Since)}");
        if (@params?.Until is not null) qs.Add($"until={Uri.EscapeDataString(@params.Until)}");
        if (@params?.CompanySearch is not null) qs.Add($"companySearch={Uri.EscapeDataString(@params.CompanySearch)}");
        if (@params?.Page is not null) qs.Add($"page={@params.Page}");
        if (@params?.PageSize is not null) qs.Add($"pageSize={@params.PageSize}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";

        return _client.RequestAsync<PagedResponse<CompanyChange>>(HttpMethod.Get, $"/v1/changes{query}", ct);
    }

    /// <summary>Get all changes for a specific company.</summary>
    public Task<List<CompanyChange>> ByCompanyAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<CompanyChange>(HttpMethod.Get, $"/v1/changes/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>Get change statistics.</summary>
    public Task<ChangeStatistics> StatisticsAsync(CancellationToken ct = default)
        => _client.RequestAsync<ChangeStatistics>(HttpMethod.Get, "/v1/changes/statistics", ct);
}
