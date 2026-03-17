using System.Text.Json;
using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Webhooks resource — CRUD and testing.</summary>
public class WebhooksResource
{
    private readonly VynCoClient _client;
    internal WebhooksResource(VynCoClient client) => _client = client;

    /// <summary>Create a new webhook endpoint.</summary>
    public Task<WebhookCreated> CreateAsync(CreateWebhookRequest request, CancellationToken ct = default)
        => _client.RequestAsync<WebhookCreated>(HttpMethod.Post, "/v1/webhooks", request, ct);

    /// <summary>List all webhooks.</summary>
    public Task<List<Webhook>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<Webhook>(HttpMethod.Get, "/v1/webhooks", ct);

    /// <summary>Get a single webhook by ID.</summary>
    public Task<Webhook> GetAsync(string webhookId, CancellationToken ct = default)
        => _client.RequestAsync<Webhook>(HttpMethod.Get, $"/v1/webhooks/{Uri.EscapeDataString(webhookId)}", ct);

    /// <summary>Delete a webhook.</summary>
    public Task DeleteAsync(string webhookId, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/webhooks/{Uri.EscapeDataString(webhookId)}", ct);

    /// <summary>Send a test event to a webhook endpoint.</summary>
    public Task<JsonElement> TestAsync(string webhookId, CancellationToken ct = default)
        => _client.RequestAsync<JsonElement>(HttpMethod.Post, $"/v1/webhooks/{Uri.EscapeDataString(webhookId)}/test", ct);
}
