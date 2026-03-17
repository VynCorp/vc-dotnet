using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Sync status resource — registry sync status.</summary>
public class SyncStatusResource
{
    private readonly VynCoClient _client;
    internal SyncStatusResource(VynCoClient client) => _client = client;

    /// <summary>Get the current sync status from the Zefix registry.</summary>
    public Task<List<SyncStatusEntry>> GetAsync(CancellationToken ct = default)
        => _client.RequestListAsync<SyncStatusEntry>(HttpMethod.Get, "/v1/sync/status", ct);
}
