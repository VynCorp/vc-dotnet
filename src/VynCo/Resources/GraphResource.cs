using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Graph resource — network graphs and analysis.</summary>
public class GraphResource
{
    private readonly VynCoClient _client;
    internal GraphResource(VynCoClient client) => _client = client;

    /// <summary>Get the network graph for a company.</summary>
    public Task<GraphResponse> GetAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<GraphResponse>(HttpMethod.Get, $"/v1/graph/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>Export a network graph in the specified format.</summary>
    public Task<ExportFile> ExportAsync(string uid, string format, CancellationToken ct = default)
        => _client.RequestBytesAsync($"/v1/graph/{Uri.EscapeDataString(uid)}/export?format={Uri.EscapeDataString(format)}", ct);

    /// <summary>Run network analysis across multiple companies.</summary>
    public Task<NetworkAnalysisResponse> AnalyzeAsync(NetworkAnalysisRequest request, CancellationToken ct = default)
        => _client.RequestAsync<NetworkAnalysisResponse>(HttpMethod.Post, "/v1/network/analyze", request, ct);
}
