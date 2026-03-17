using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Dossiers resource — generate, get, list AI-powered company dossiers.</summary>
public class DossiersResource
{
    private readonly VynCoClient _client;
    internal DossiersResource(VynCoClient client) => _client = client;

    /// <summary>Get a dossier for a company.</summary>
    public Task<Dossier> GetAsync(string companyUid, CancellationToken ct = default)
        => _client.RequestAsync<Dossier>(HttpMethod.Get, $"/v1/dossiers/{Uri.EscapeDataString(companyUid)}", ct);

    /// <summary>List all dossiers.</summary>
    public Task<List<Dossier>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<Dossier>(HttpMethod.Get, "/v1/dossiers", ct);

    /// <summary>Generate an AI-powered company dossier (returns 202 Accepted).</summary>
    public Task<DossierGenerated> GenerateAsync(string companyUid, CancellationToken ct = default)
        => _client.RequestAsync<DossierGenerated>(HttpMethod.Post, $"/v1/dossiers/{Uri.EscapeDataString(companyUid)}/generate", ct);

    /// <summary>Get dossier generation statistics.</summary>
    public Task<DossierStatistics> StatisticsAsync(CancellationToken ct = default)
        => _client.RequestAsync<DossierStatistics>(HttpMethod.Get, "/v1/dossiers/statistics", ct);
}
