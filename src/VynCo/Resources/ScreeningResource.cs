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
}
