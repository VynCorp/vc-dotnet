using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Canton distribution analytics.</summary>
public class CantonDistribution
{
    [JsonPropertyName("canton")] public string Canton { get; set; } = "";
    [JsonPropertyName("count")] public long Count { get; set; }
    [JsonPropertyName("percentage")] public double Percentage { get; set; }
}

/// <summary>Auditor market share analytics.</summary>
public class AuditorMarketShare
{
    [JsonPropertyName("auditorName")] public string AuditorName { get; set; } = "";
    [JsonPropertyName("companyCount")] public long CompanyCount { get; set; }
    [JsonPropertyName("percentage")] public double Percentage { get; set; }
}

/// <summary>Request body for clustering analysis.</summary>
public class ClusterRequest
{
    [JsonPropertyName("algorithm")] public string Algorithm { get; set; } = "";
    [JsonPropertyName("k")] public int? K { get; set; }
}

/// <summary>Clustering analysis response.</summary>
public class ClusterResponse
{
    [JsonPropertyName("clusters")] public List<ClusterResult> Clusters { get; set; } = new();
}

/// <summary>A single cluster result.</summary>
public class ClusterResult
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("centroid")] public JsonElement Centroid { get; set; }
    [JsonPropertyName("companyCount")] public long CompanyCount { get; set; }
    [JsonPropertyName("sampleCompanies")] public List<string> SampleCompanies { get; set; } = new();
}

/// <summary>Request body for anomaly detection.</summary>
public class AnomalyRequest
{
    [JsonPropertyName("algorithm")] public string Algorithm { get; set; } = "";
    [JsonPropertyName("threshold")] public double? Threshold { get; set; }
}

/// <summary>Anomaly detection response.</summary>
public class AnomalyResponse
{
    [JsonPropertyName("anomalies")] public List<JsonElement> Anomalies { get; set; } = new();
    [JsonPropertyName("totalScanned")] public long TotalScanned { get; set; }
    [JsonPropertyName("threshold")] public double Threshold { get; set; }
}

/// <summary>RFM segmentation response.</summary>
public class RfmSegmentsResponse
{
    [JsonPropertyName("segments")] public List<RfmSegment> Segments { get; set; } = new();
}

/// <summary>A single RFM segment.</summary>
public class RfmSegment
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("count")] public long Count { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = "";
}

/// <summary>Query parameters for cohort analysis.</summary>
public class CohortParams
{
    public string? GroupBy { get; set; }
    public string? Metric { get; set; }
}

/// <summary>Cohort analysis response.</summary>
public class CohortResponse
{
    [JsonPropertyName("cohorts")] public List<CohortEntry> Cohorts { get; set; } = new();
    [JsonPropertyName("groupBy")] public string GroupBy { get; set; } = "";
    [JsonPropertyName("metric")] public string Metric { get; set; } = "";
}

/// <summary>A single cohort entry.</summary>
public class CohortEntry
{
    [JsonPropertyName("group")] public string Group { get; set; } = "";
    [JsonPropertyName("count")] public long Count { get; set; }
    [JsonPropertyName("metric")] public string Metric { get; set; } = "";
}

/// <summary>Query parameters for audit candidate listing.</summary>
public class CandidateParams
{
    public string? SortBy { get; set; }
    public string? Canton { get; set; }
    public long? Page { get; set; }
    public long? PageSize { get; set; }
}

/// <summary>An audit candidate company.</summary>
public class AuditCandidate
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("legalForm")] public string? LegalForm { get; set; }
    [JsonPropertyName("shareCapital")] public double? ShareCapital { get; set; }
    [JsonPropertyName("auditorName")] public string? AuditorName { get; set; }
    [JsonPropertyName("auditorCategory")] public string? AuditorCategory { get; set; }
}
