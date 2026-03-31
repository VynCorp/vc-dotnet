using VynCo.Models;

namespace VynCo.Resources;

/// <summary>AI resource — dossier generation, search, risk scoring.</summary>
public class AiResource
{
    private readonly VynCoClient _client;
    internal AiResource(VynCoClient client) => _client = client;

    /// <summary>Generate an AI-powered dossier for a company.</summary>
    public Task<AiDossierResponse> DossierAsync(AiDossierRequest request, CancellationToken ct = default)
        => _client.RequestAsync<AiDossierResponse>(HttpMethod.Post, "/v1/ai/dossier", request, ct);

    /// <summary>AI-powered natural language search for companies.</summary>
    public Task<AiSearchResponse> SearchAsync(AiSearchRequest request, CancellationToken ct = default)
        => _client.RequestAsync<AiSearchResponse>(HttpMethod.Post, "/v1/ai/search", request, ct);

    /// <summary>Get an AI risk score for a company.</summary>
    public Task<RiskScoreResponse> RiskScoreAsync(RiskScoreRequest request, CancellationToken ct = default)
        => _client.RequestAsync<RiskScoreResponse>(HttpMethod.Post, "/v1/ai/risk-score", request, ct);
}
