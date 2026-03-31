using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Webhooks resource — manage webhook subscriptions.</summary>
public class WebhooksResource
{
    private readonly VynCoClient _client;
    internal WebhooksResource(VynCoClient client) => _client = client;

    /// <summary>List all webhook subscriptions.</summary>
    public Task<List<WebhookSubscription>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<WebhookSubscription>(HttpMethod.Get, "/v1/webhooks", ct);

    /// <summary>Create a new webhook subscription.</summary>
    public Task<CreateWebhookResponse> CreateAsync(CreateWebhookRequest request, CancellationToken ct = default)
        => _client.RequestAsync<CreateWebhookResponse>(HttpMethod.Post, "/v1/webhooks", request, ct);

    /// <summary>Update a webhook subscription.</summary>
    public Task<WebhookSubscription> UpdateAsync(string id, UpdateWebhookRequest request, CancellationToken ct = default)
        => _client.RequestAsync<WebhookSubscription>(HttpMethod.Put, $"/v1/webhooks/{Uri.EscapeDataString(id)}", request, ct);

    /// <summary>Delete a webhook subscription.</summary>
    public Task DeleteAsync(string id, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/webhooks/{Uri.EscapeDataString(id)}", ct);

    /// <summary>Test a webhook by sending a test delivery.</summary>
    public Task<TestDeliveryResponse> TestAsync(string id, CancellationToken ct = default)
        => _client.RequestAsync<TestDeliveryResponse>(HttpMethod.Post, $"/v1/webhooks/{Uri.EscapeDataString(id)}/test", new { }, ct);

    /// <summary>List deliveries for a webhook.</summary>
    public Task<List<WebhookDelivery>> DeliveriesAsync(string id, int? limit = null, CancellationToken ct = default)
    {
        var query = limit.HasValue ? $"?limit={limit.Value}" : "";
        return _client.RequestListAsync<WebhookDelivery>(HttpMethod.Get, $"/v1/webhooks/{Uri.EscapeDataString(id)}/deliveries{query}", ct);
    }
}
