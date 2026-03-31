using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Auditor history for a company.</summary>
public class AuditorHistoryResponse
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("currentAuditor")] public AuditorTenure? CurrentAuditor { get; set; }
    [JsonPropertyName("history")] public List<AuditorTenure> History { get; set; } = new();
}

/// <summary>A single auditor tenure record.</summary>
public class AuditorTenure
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("auditorName")] public string AuditorName { get; set; } = "";
    [JsonPropertyName("appointedAt")] public string? AppointedAt { get; set; }
    [JsonPropertyName("resignedAt")] public string? ResignedAt { get; set; }
    [JsonPropertyName("tenureYears")] public double? TenureYears { get; set; }
    [JsonPropertyName("isCurrent")] public bool IsCurrent { get; set; }
    [JsonPropertyName("source")] public string Source { get; set; } = "";
}

/// <summary>Query parameters for auditor tenure listing.</summary>
public class AuditorTenureParams
{
    public double? MinYears { get; set; }
    public string? Canton { get; set; }
    public long? Page { get; set; }
    public long? PageSize { get; set; }
}
