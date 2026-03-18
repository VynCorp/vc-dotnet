using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Changes resource — list, get by company, statistics, review, batch, SOGC lookup.</summary>
public class ChangesResource
{
    private readonly VynCoClient _client;
    internal ChangesResource(VynCoClient client) => _client = client;

    /// <summary>List recent company changes with pagination.</summary>
    public Task<PagedResponse<Change>> ListAsync(ListChangesParams? @params = null, CancellationToken ct = default)
    {
        var p = @params ?? new ListChangesParams();
        var query = $"?page={p.Page}&pageSize={p.PageSize}";
        if (p.CompanyUid is not null) query += $"&companyUid={Uri.EscapeDataString(p.CompanyUid)}";

        return _client.RequestAsync<PagedResponse<Change>>(HttpMethod.Get, $"/api/v1/changes{query}", ct);
    }

    /// <summary>Get all changes for a specific company.</summary>
    public Task<List<Change>> GetByCompanyAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<Change>(HttpMethod.Get, $"/api/v1/changes/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>Get change statistics (totals, reviewed, flagged, by type).</summary>
    public Task<ChangeStatistics> StatisticsAsync(CancellationToken ct = default)
        => _client.RequestAsync<ChangeStatistics>(HttpMethod.Get, "/api/v1/changes/statistics", ct);

    /// <summary>Get all changes linked to a SOGC (Swiss Official Gazette of Commerce) publication.</summary>
    public Task<List<Change>> GetBySogcIdAsync(string sogcId, CancellationToken ct = default)
        => _client.RequestListAsync<Change>(HttpMethod.Get, $"/api/v1/changes/sogc/{Uri.EscapeDataString(sogcId)}", ct);

    /// <summary>Mark a change as reviewed with optional notes.</summary>
    public Task<ReviewResult> ReviewAsync(Guid changeId, ReviewChangeRequest request, CancellationToken ct = default)
        => _client.RequestAsync<ReviewResult>(HttpMethod.Put, $"/api/v1/changes/{changeId}/review", request, ct);

    /// <summary>Batch get changes for multiple company UIDs.</summary>
    public Task<List<Change>> BatchAsync(BatchLookupRequest request, CancellationToken ct = default)
        => _client.RequestListAsync<Change>(HttpMethod.Post, "/api/v1/changes/batch", request, ct);
}
