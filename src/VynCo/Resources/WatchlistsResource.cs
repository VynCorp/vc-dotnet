using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Watchlists resource — manage watchlists and their companies.</summary>
public class WatchlistsResource
{
    private readonly VynCoClient _client;
    internal WatchlistsResource(VynCoClient client) => _client = client;

    /// <summary>List all watchlists.</summary>
    public Task<List<WatchlistSummary>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<WatchlistSummary>(HttpMethod.Get, "/v1/watchlists", ct);

    /// <summary>Create a new watchlist.</summary>
    public Task<Watchlist> CreateAsync(CreateWatchlistRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Watchlist>(HttpMethod.Post, "/v1/watchlists", request, ct);

    /// <summary>Delete a watchlist.</summary>
    public Task DeleteAsync(string id, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/watchlists/{Uri.EscapeDataString(id)}", ct);

    /// <summary>Get the company UIDs in a watchlist.</summary>
    public Task<WatchlistCompaniesResponse> CompaniesAsync(string id, CancellationToken ct = default)
        => _client.RequestAsync<WatchlistCompaniesResponse>(HttpMethod.Get, $"/v1/watchlists/{Uri.EscapeDataString(id)}/companies", ct);

    /// <summary>Add companies to a watchlist.</summary>
    public Task<AddCompaniesResponse> AddCompaniesAsync(string id, AddCompaniesRequest request, CancellationToken ct = default)
        => _client.RequestAsync<AddCompaniesResponse>(HttpMethod.Post, $"/v1/watchlists/{Uri.EscapeDataString(id)}/companies", request, ct);

    /// <summary>Remove a company from a watchlist.</summary>
    public Task RemoveCompanyAsync(string id, string uid, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/watchlists/{Uri.EscapeDataString(id)}/companies/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>Get events for a watchlist.</summary>
    public Task<EventListResponse> EventsAsync(string id, int? limit = null, CancellationToken ct = default)
    {
        var query = limit.HasValue ? $"?limit={limit.Value}" : "";
        return _client.RequestAsync<EventListResponse>(HttpMethod.Get, $"/v1/watchlists/{Uri.EscapeDataString(id)}/events{query}", ct);
    }
}
