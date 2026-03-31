using VynCo.Models;

namespace VynCo.Resources;

/// <summary>API key management resource.</summary>
public class ApiKeysResource
{
    private readonly VynCoClient _client;
    internal ApiKeysResource(VynCoClient client) => _client = client;

    /// <summary>List all API keys.</summary>
    public Task<List<ApiKey>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<ApiKey>(HttpMethod.Get, "/v1/api-keys", ct);

    /// <summary>Create a new API key.</summary>
    public Task<ApiKeyCreated> CreateAsync(CreateApiKeyRequest request, CancellationToken ct = default)
        => _client.RequestAsync<ApiKeyCreated>(HttpMethod.Post, "/v1/api-keys", request, ct);

    /// <summary>Revoke an API key.</summary>
    public Task RevokeAsync(string id, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/api-keys/{Uri.EscapeDataString(id)}", ct);
}
