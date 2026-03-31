using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Dossiers resource — create, list, get, delete managed dossiers.</summary>
public class DossiersResource
{
    private readonly VynCoClient _client;
    internal DossiersResource(VynCoClient client) => _client = client;

    /// <summary>Create a new dossier.</summary>
    public Task<Dossier> CreateAsync(CreateDossierRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Dossier>(HttpMethod.Post, "/v1/dossiers", request, ct);

    /// <summary>List all dossiers.</summary>
    public Task<List<DossierSummary>> ListAsync(CancellationToken ct = default)
        => _client.RequestListAsync<DossierSummary>(HttpMethod.Get, "/v1/dossiers", ct);

    /// <summary>Get a dossier by ID or company UID.</summary>
    public Task<Dossier> GetAsync(string idOrUid, CancellationToken ct = default)
        => _client.RequestAsync<Dossier>(HttpMethod.Get, $"/v1/dossiers/{Uri.EscapeDataString(idOrUid)}", ct);

    /// <summary>Delete a dossier.</summary>
    public Task DeleteAsync(string id, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/dossiers/{Uri.EscapeDataString(id)}", ct);
}
