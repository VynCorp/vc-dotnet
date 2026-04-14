using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Saved searches resource — manage persistent search queries.</summary>
public class SavedSearchesResource
{
    private readonly VynCoClient _client;
    internal SavedSearchesResource(VynCoClient client) => _client = client;

    /// <summary>List all saved searches.</summary>
    public Task<List<SavedSearch>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<SavedSearch>(HttpMethod.Get, "/v1/saved-searches", ct);

    /// <summary>Create a new saved search.</summary>
    public Task<SavedSearch> CreateAsync(CreateSavedSearchRequest request, CancellationToken ct = default)
        => _client.RequestAsync<SavedSearch>(HttpMethod.Post, "/v1/saved-searches", request, ct);

    /// <summary>Get a saved search by ID.</summary>
    public Task<SavedSearch> GetAsync(string id, CancellationToken ct = default)
        => _client.RequestAsync<SavedSearch>(HttpMethod.Get, $"/v1/saved-searches/{Uri.EscapeDataString(id)}", ct);

    /// <summary>Update a saved search.</summary>
    public Task<SavedSearch> UpdateAsync(string id, UpdateSavedSearchRequest request, CancellationToken ct = default)
        => _client.RequestAsync<SavedSearch>(HttpMethod.Put, $"/v1/saved-searches/{Uri.EscapeDataString(id)}", request, ct);

    /// <summary>Delete a saved search.</summary>
    public Task DeleteAsync(string id, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/saved-searches/{Uri.EscapeDataString(id)}", ct);
}
