using VynCo.Models;

namespace VynCo.Resources;

/// <summary>News resource — recent news across all companies.</summary>
public class NewsResource
{
    private readonly VynCoClient _client;
    internal NewsResource(VynCoClient client) => _client = client;

    /// <summary>Get recent news across all companies. Cost: 1 credit.</summary>
    public Task<RecentNewsResponse> GetRecentAsync(int limit = 50, CancellationToken ct = default)
        => _client.RequestAsync<RecentNewsResponse>(HttpMethod.Get, $"/api/v1/news/recent?limit={limit}", ct);
}
