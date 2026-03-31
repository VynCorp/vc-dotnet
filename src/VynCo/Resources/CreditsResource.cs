using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Credits resource — balance, usage, history.</summary>
public class CreditsResource
{
    private readonly VynCoClient _client;
    internal CreditsResource(VynCoClient client) => _client = client;

    /// <summary>Get current credit balance and tier info.</summary>
    public Task<CreditBalance> BalanceAsync(CancellationToken ct = default)
        => _client.RequestAsync<CreditBalance>(HttpMethod.Get, "/v1/credits/balance", ct);

    /// <summary>Get usage breakdown by operation type.</summary>
    public Task<CreditUsage> UsageAsync(string? since = null, CancellationToken ct = default)
    {
        var query = since is not null ? $"?since={Uri.EscapeDataString(since)}" : "";
        return _client.RequestAsync<CreditUsage>(HttpMethod.Get, $"/v1/credits/usage{query}", ct);
    }

    /// <summary>Get credit ledger history.</summary>
    public Task<CreditHistory> HistoryAsync(long? limit = null, long? offset = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (limit.HasValue) qs.Add($"limit={limit.Value}");
        if (offset.HasValue) qs.Add($"offset={offset.Value}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";

        return _client.RequestAsync<CreditHistory>(HttpMethod.Get, $"/v1/credits/history{query}", ct);
    }
}
