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

    /// <summary>Get AI risk scores for up to 50 companies in a single call (v3.1+).</summary>
    public Task<BatchRiskScoreResponse> RiskScoreBatchAsync(BatchRiskScoreRequest request, CancellationToken ct = default)
        => _client.RequestAsync<BatchRiskScoreResponse>(HttpMethod.Post, "/v1/ai/risk-score/batch", request, ct);

    /// <summary>Generate an AI comparative dossier for 2-5 companies.</summary>
    public Task<ComparativeResponse> ComparativeAsync(ComparativeRequest request, CancellationToken ct = default)
        => _client.RequestAsync<ComparativeResponse>(HttpMethod.Post, "/v1/ai/comparative", request, ct);

    /// <summary>Get predictive risk scoring with dissolution probability for a company.</summary>
    public Task<PredictiveRiskResponse> PredictiveRiskAsync(string uid, PredictiveRiskRequest? request = null, CancellationToken ct = default)
        => _client.RequestAsync<PredictiveRiskResponse>(HttpMethod.Post, $"/v1/risk/predictive/{Uri.EscapeDataString(uid)}", request, ct);
}
