using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Screening resource — compliance screening.</summary>
public class ScreeningResource
{
    private readonly VynCoClient _client;
    internal ScreeningResource(VynCoClient client) => _client = client;

    /// <summary>Screen a company or entity against sanctions and compliance databases.</summary>
    public Task<ScreeningResponse> ScreenAsync(ScreeningRequest request, CancellationToken ct = default)
        => _client.RequestAsync<ScreeningResponse>(HttpMethod.Post, "/v1/screening", request, ct);

    /// <summary>Screen up to 100 companies against sanctions lists in a single call (v3.1+).</summary>
    public Task<BatchScreeningResponse> BatchAsync(BatchScreeningRequest request, CancellationToken ct = default)
        => _client.RequestAsync<BatchScreeningResponse>(HttpMethod.Post, "/v1/screening/batch", request, ct);

    /// <summary>Browse SECO/OpenSanctions/FINMA sanctions databases with search and pagination.</summary>
    public Task<SanctionsListResponse> BrowseSanctionsAsync(SanctionsSearchParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.Search is not null) qs.Add($"search={Uri.EscapeDataString(@params.Search)}");
        if (@params?.EntityType is not null) qs.Add($"entityType={Uri.EscapeDataString(@params.EntityType)}");
        if (@params?.Program is not null) qs.Add($"program={Uri.EscapeDataString(@params.Program)}");
        if (@params?.Page is not null) qs.Add($"page={@params.Page}");
        if (@params?.PageSize is not null) qs.Add($"pageSize={@params.PageSize}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";
        return _client.RequestAsync<SanctionsListResponse>(HttpMethod.Get, $"/v1/sanctions{query}", ct);
    }
}
