using System.Text.Json;
using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Dossiers resource — generate, get, list AI-powered company dossiers.</summary>
public class DossiersResource
{
    private readonly VynCoClient _client;
    internal DossiersResource(VynCoClient client) => _client = client;

    /// <summary>Get a dossier for a company.</summary>
    public Task<Dossier> GetAsync(string companyUid, CancellationToken ct = default)
        => _client.RequestAsync<Dossier>(HttpMethod.Get, $"/api/v1/dossiers/{Uri.EscapeDataString(companyUid)}", ct);

    /// <summary>List all dossiers.</summary>
    public Task<List<Dossier>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<Dossier>(HttpMethod.Get, "/api/v1/dossiers", ct);

    /// <summary>Generate an AI-powered company dossier. Cost: 40 credits (standard) or 100 credits (comprehensive).</summary>
    public Task<Dossier> GenerateAsync(string companyUid, GenerateDossierRequest? request = null, CancellationToken ct = default)
        => _client.RequestAsync<Dossier>(HttpMethod.Post, $"/api/v1/dossiers/{Uri.EscapeDataString(companyUid)}/generate", request, ct);

    /// <summary>Get dossier generation statistics.</summary>
    public Task<JsonElement> StatisticsAsync(CancellationToken ct = default)
        => _client.RequestAsync<JsonElement>(HttpMethod.Get, "/api/v1/dossiers/statistics", ct);
}
