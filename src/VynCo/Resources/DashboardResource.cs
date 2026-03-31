using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Dashboard resource — admin dashboard.</summary>
public class DashboardResource
{
    private readonly VynCoClient _client;
    internal DashboardResource(VynCoClient client) => _client = client;

    /// <summary>Get the admin dashboard.</summary>
    public Task<DashboardResponse> GetAsync(CancellationToken ct = default)
        => _client.RequestAsync<DashboardResponse>(HttpMethod.Get, "/v1/dashboard", ct);
}
