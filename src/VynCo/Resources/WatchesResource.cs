using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Watches resource — subscribe to company change notifications.</summary>
public class WatchesResource
{
    private readonly VynCoClient _client;
    internal WatchesResource(VynCoClient client) => _client = client;

    /// <summary>List all watched companies for the authenticated team.</summary>
    public Task<List<CompanyWatch>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<CompanyWatch>(HttpMethod.Get, "/api/v1/watches", ct);

    /// <summary>Subscribe to change notifications for a company.</summary>
    public Task<CompanyWatch> AddAsync(AddWatchRequest request, CancellationToken ct = default)
        => _client.RequestAsync<CompanyWatch>(HttpMethod.Post, "/api/v1/watches", request, ct);

    /// <summary>Remove a company watch subscription.</summary>
    public Task RemoveAsync(string companyUid, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/api/v1/watches/{Uri.EscapeDataString(companyUid)}", ct);

    /// <summary>List recent change notifications for the authenticated team.</summary>
    public Task<List<ChangeNotification>> ListNotificationsAsync(int limit = 50, CancellationToken ct = default)
        => _client.RequestListAsync<ChangeNotification>(HttpMethod.Get, $"/api/v1/notifications?limit={limit}", ct);
}
