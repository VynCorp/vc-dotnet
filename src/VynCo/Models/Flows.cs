using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Query parameters for <c>analytics.FlowsAsync</c> (v3.1+).</summary>
public class FlowsParams
{
    /// <summary><c>monthly</c> (default) | <c>quarterly</c> | <c>yearly</c>.</summary>
    public string? Period { get; set; }
    public string? Since { get; set; }
    /// <summary><c>canton</c> (default) | <c>industry</c> | <c>legalForm</c>.</summary>
    public string? GroupBy { get; set; }
}

/// <summary>A single period of company registration/dissolution flow (v3.1+).</summary>
public class FlowDataPoint
{
    [JsonPropertyName("period")] public string Period { get; set; } = "";
    [JsonPropertyName("group")] public string Group { get; set; } = "";
    [JsonPropertyName("registrations")] public long Registrations { get; set; }
    [JsonPropertyName("dissolutions")] public long Dissolutions { get; set; }
    [JsonPropertyName("net")] public long Net { get; set; }
}

/// <summary>Market flow analytics response (v3.1+).</summary>
public class FlowsResponse
{
    [JsonPropertyName("flows")] public List<FlowDataPoint> Flows { get; set; } = new();
    /// <summary>Surfaces asymmetric-accuracy notes (e.g. historical dissolution under-counting).</summary>
    [JsonPropertyName("dataCoverageNote")] public string? DataCoverageNote { get; set; }
}

/// <summary>Query parameters for <c>analytics.MigrationsAsync</c> (v3.1+).</summary>
public class MigrationsParams
{
    public string? Since { get; set; }
}

/// <summary>A single canton-to-canton migration flow (v3.1+).</summary>
public class MigrationFlow
{
    [JsonPropertyName("fromCanton")] public string FromCanton { get; set; } = "";
    [JsonPropertyName("toCanton")] public string ToCanton { get; set; } = "";
    [JsonPropertyName("count")] public long Count { get; set; }
}

/// <summary>Canton migration analytics response (v3.1+).</summary>
public class MigrationResponse
{
    [JsonPropertyName("flows")] public List<MigrationFlow> Flows { get; set; } = new();
    [JsonPropertyName("topFlows")] public List<MigrationFlow> TopFlows { get; set; } = new();
}

/// <summary>Query parameters for <c>analytics.BenchmarkAsync</c> (v3.1+).</summary>
public class BenchmarkParams
{
    /// <summary>Comma-separated dimensions (e.g. <c>capital,board_size</c>). Omit for all.</summary>
    public string? Dimensions { get; set; }
}

/// <summary>A single benchmarking dimension (v3.1+).</summary>
public class BenchmarkDimension
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("companyValue")] public double CompanyValue { get; set; }
    [JsonPropertyName("industryMedian")] public double IndustryMedian { get; set; }
    [JsonPropertyName("percentile")] public double Percentile { get; set; }
}

/// <summary>Industry benchmarking response — how a company compares to peers (v3.1+).</summary>
public class BenchmarkResponse
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("industry")] public string? Industry { get; set; }
    [JsonPropertyName("peerCount")] public long PeerCount { get; set; }
    [JsonPropertyName("dimensions")] public List<BenchmarkDimension> Dimensions { get; set; } = new();
}
