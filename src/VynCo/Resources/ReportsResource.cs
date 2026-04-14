using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Reports resource — industry reports and AI-generated narratives.</summary>
public class ReportsResource
{
    private readonly VynCoClient _client;
    internal ReportsResource(VynCoClient client) => _client = client;

    /// <summary>List all industries with available reports and company counts.</summary>
    public Task<IndustryListResponse> IndustriesAsync(CancellationToken ct = default)
        => _client.RequestAsync<IndustryListResponse>(HttpMethod.Get, "/v1/reports/industries", ct);

    /// <summary>Get a detailed industry report with analytics.</summary>
    public Task<IndustryReportResponse> GetAsync(string industry, CancellationToken ct = default)
        => _client.RequestAsync<IndustryReportResponse>(HttpMethod.Get, $"/v1/reports/industry/{Uri.EscapeDataString(industry)}", ct);

    /// <summary>Generate an AI-powered narrative industry report.</summary>
    public Task<GeneratedIndustryReport> GenerateAsync(string industry, CancellationToken ct = default)
        => _client.RequestAsync<GeneratedIndustryReport>(HttpMethod.Post, $"/v1/reports/industry/{Uri.EscapeDataString(industry)}/generate", ct);
}
