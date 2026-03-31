using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Exports resource — create, get, and download data exports.</summary>
public class ExportsResource
{
    private readonly VynCoClient _client;
    internal ExportsResource(VynCoClient client) => _client = client;

    /// <summary>Create a data export job.</summary>
    public Task<ExportJob> CreateAsync(CreateExportRequest request, CancellationToken ct = default)
        => _client.RequestAsync<ExportJob>(HttpMethod.Post, "/v1/exports", request, ct);

    /// <summary>Get export job status and metadata.</summary>
    public Task<ExportDownload> GetAsync(string id, CancellationToken ct = default)
        => _client.RequestAsync<ExportDownload>(HttpMethod.Get, $"/v1/exports/{Uri.EscapeDataString(id)}", ct);

    /// <summary>Download the exported file as raw bytes.</summary>
    public Task<ExportFile> DownloadAsync(string id, CancellationToken ct = default)
        => _client.RequestBytesAsync($"/v1/exports/{Uri.EscapeDataString(id)}/download", ct);
}
