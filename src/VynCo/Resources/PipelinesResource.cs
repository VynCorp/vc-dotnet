using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Pipelines resource — sales/prospect tracking pipelines.</summary>
public class PipelinesResource
{
    private readonly VynCoClient _client;
    internal PipelinesResource(VynCoClient client) => _client = client;

    /// <summary>List all pipelines.</summary>
    public Task<List<Pipeline>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<Pipeline>(HttpMethod.Get, "/v1/pipelines", ct);

    /// <summary>Create a new pipeline with optional custom stages.</summary>
    public Task<Pipeline> CreateAsync(CreatePipelineRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Pipeline>(HttpMethod.Post, "/v1/pipelines", request, ct);

    /// <summary>Get a pipeline with all its entries.</summary>
    public Task<PipelineWithEntries> GetAsync(string id, CancellationToken ct = default)
        => _client.RequestAsync<PipelineWithEntries>(HttpMethod.Get, $"/v1/pipelines/{Uri.EscapeDataString(id)}", ct);

    /// <summary>Delete a pipeline.</summary>
    public Task DeleteAsync(string id, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/pipelines/{Uri.EscapeDataString(id)}", ct);

    /// <summary>Add a company entry to a pipeline.</summary>
    public Task<PipelineEntry> AddEntryAsync(string id, AddEntryRequest request, CancellationToken ct = default)
        => _client.RequestAsync<PipelineEntry>(HttpMethod.Post, $"/v1/pipelines/{Uri.EscapeDataString(id)}/entries", request, ct);

    /// <summary>Update a pipeline entry.</summary>
    public Task<PipelineEntry> UpdateEntryAsync(string id, string entryId, UpdateEntryRequest request, CancellationToken ct = default)
        => _client.RequestAsync<PipelineEntry>(HttpMethod.Put, $"/v1/pipelines/{Uri.EscapeDataString(id)}/entries/{Uri.EscapeDataString(entryId)}", request, ct);

    /// <summary>Remove an entry from a pipeline.</summary>
    public Task RemoveEntryAsync(string id, string entryId, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/pipelines/{Uri.EscapeDataString(id)}/entries/{Uri.EscapeDataString(entryId)}", ct);

    /// <summary>Get aggregate statistics for a pipeline.</summary>
    public Task<PipelineStats> StatsAsync(string id, CancellationToken ct = default)
        => _client.RequestAsync<PipelineStats>(HttpMethod.Get, $"/v1/pipelines/{Uri.EscapeDataString(id)}/stats", ct);
}
