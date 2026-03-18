using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Credits resource — balance, usage, history.</summary>
public class CreditsResource
{
    private readonly VynCoClient _client;
    internal CreditsResource(VynCoClient client) => _client = client;

    /// <summary>Get current credit balance and tier info.</summary>
    public Task<CreditBalance> BalanceAsync(CancellationToken ct = default)
        => _client.RequestAsync<CreditBalance>(HttpMethod.Get, "/api/v1/credits/balance", ct);

    /// <summary>Get usage breakdown by operation type.</summary>
    public Task<UsageBreakdown> UsageAsync(CreditUsageParams? @params = null, CancellationToken ct = default)
    {
        var query = "";
        if (@params?.Since is not null)
            query = $"?since={Uri.EscapeDataString(@params.Since)}";

        return _client.RequestAsync<UsageBreakdown>(HttpMethod.Get, $"/api/v1/credits/usage{query}", ct);
    }

    /// <summary>Get credit ledger entries (transaction history).</summary>
    public Task<List<CreditLedgerEntry>> HistoryAsync(CreditHistoryParams? @params = null, CancellationToken ct = default)
    {
        var p = @params ?? new CreditHistoryParams();
        var query = $"?limit={p.Limit}&offset={p.Offset}";

        return _client.RequestListAsync<CreditLedgerEntry>(HttpMethod.Get, $"/api/v1/credits/history{query}", ct);
    }
}
