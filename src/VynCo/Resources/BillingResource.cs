using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Billing resource — Stripe checkout and portal sessions.</summary>
public class BillingResource
{
    private readonly VynCoClient _client;
    internal BillingResource(VynCoClient client) => _client = client;

    /// <summary>Create a Stripe checkout session for upgrading a plan.</summary>
    public Task<SessionUrl> CreateCheckoutSessionAsync(CancellationToken ct = default)
        => _client.RequestAsync<SessionUrl>(HttpMethod.Post, "/v1/billing/checkout-session", ct);

    /// <summary>Create a Stripe customer portal session for managing billing.</summary>
    public Task<SessionUrl> CreatePortalSessionAsync(CancellationToken ct = default)
        => _client.RequestAsync<SessionUrl>(HttpMethod.Post, "/v1/billing/portal-session", ct);
}
