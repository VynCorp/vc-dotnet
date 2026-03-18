using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Health resource — API health check.</summary>
public class HealthResource
{
    private readonly VynCoClient _client;
    internal HealthResource(VynCoClient client) => _client = client;

    /// <summary>Check the operational status of the API and its dependencies. No authentication or credits required.</summary>
    public Task<HealthResponse> CheckAsync(CancellationToken ct = default)
        => _client.RequestAsync<HealthResponse>(HttpMethod.Get, "/api/v1/health", ct);
}
