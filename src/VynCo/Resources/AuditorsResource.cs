using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Auditors resource — auditor history and tenures.</summary>
public class AuditorsResource
{
    private readonly VynCoClient _client;
    internal AuditorsResource(VynCoClient client) => _client = client;

    /// <summary>Get auditor history for a company.</summary>
    public Task<AuditorHistoryResponse> HistoryAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<AuditorHistoryResponse>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/auditor-history", ct);

    /// <summary>List auditor tenures with optional filtering.</summary>
    public Task<PagedResponse<AuditorTenure>> TenuresAsync(AuditorTenureParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.MinYears.HasValue == true) qs.Add($"minYears={@params.MinYears.Value}");
        if (@params?.Canton is not null) qs.Add($"canton={Uri.EscapeDataString(@params.Canton)}");
        if (@params?.Page is not null) qs.Add($"page={@params.Page}");
        if (@params?.PageSize is not null) qs.Add($"pageSize={@params.PageSize}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";

        return _client.RequestAsync<PagedResponse<AuditorTenure>>(HttpMethod.Get, $"/v1/auditor-tenures{query}", ct);
    }
}
