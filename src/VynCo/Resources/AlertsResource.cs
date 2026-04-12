using VynCo.Models;

namespace VynCo.Resources;

/// <summary>
/// Saved alerts — persistent saved queries that trigger notifications
/// (optionally via webhook) when matching companies or events appear (v3.1+).
/// </summary>
public class AlertsResource
{
    private readonly VynCoClient _client;
    internal AlertsResource(VynCoClient client) => _client = client;

    /// <summary>List all alerts for the authenticated user.</summary>
    public Task<List<Alert>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<Alert>(HttpMethod.Get, "/v1/alerts", ct);

    /// <summary>
    /// Create a new alert. <c>Frequency</c> accepts <c>hourly</c>, <c>daily</c>,
    /// or <c>weekly</c> (default <c>daily</c> on the server).
    /// </summary>
    public Task<Alert> CreateAsync(CreateAlertRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Alert>(HttpMethod.Post, "/v1/alerts", request, ct);

    /// <summary>Delete an alert.</summary>
    public Task DeleteAsync(string id, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/alerts/{Uri.EscapeDataString(id)}", ct);
}
