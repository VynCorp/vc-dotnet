using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Billing resource — Stripe checkout and portal sessions.</summary>
public class BillingResource
{
    private readonly VynCoClient _client;
    internal BillingResource(VynCoClient client) => _client = client;

    /// <summary>Create a Stripe checkout session.</summary>
    public Task<SessionUrl> CreateCheckoutAsync(CheckoutRequest request, CancellationToken ct = default)
        => _client.RequestAsync<SessionUrl>(HttpMethod.Post, "/v1/billing/checkout-session", request, ct);

    /// <summary>Create a Stripe customer portal session.</summary>
    public Task<SessionUrl> CreatePortalAsync(CancellationToken ct = default)
        => _client.RequestAsync<SessionUrl>(HttpMethod.Post, "/v1/billing/portal-session", new { }, ct);
}
