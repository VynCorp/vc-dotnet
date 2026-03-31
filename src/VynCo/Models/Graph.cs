using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Network graph response.</summary>
public class GraphResponse
{
    [JsonPropertyName("nodes")] public List<GraphNode> Nodes { get; set; } = new();
    [JsonPropertyName("links")] public List<GraphLink> Links { get; set; } = new();
}

/// <summary>A node in the network graph.</summary>
public class GraphNode
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("type")] public string NodeType { get; set; } = "";
    [JsonPropertyName("capital")] public double? Capital { get; set; }
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("role")] public string? Role { get; set; }
    [JsonPropertyName("personId")] public string? PersonId { get; set; }
}

/// <summary>A link in the network graph.</summary>
public class GraphLink
{
    [JsonPropertyName("source")] public string Source { get; set; } = "";
    [JsonPropertyName("target")] public string Target { get; set; } = "";
    [JsonPropertyName("type")] public string LinkType { get; set; } = "";
    [JsonPropertyName("label")] public string Label { get; set; } = "";
}

/// <summary>Request body for network analysis.</summary>
public class NetworkAnalysisRequest
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
    [JsonPropertyName("overlay")] public string Overlay { get; set; } = "";
}

/// <summary>Network analysis response.</summary>
public class NetworkAnalysisResponse
{
    [JsonPropertyName("nodes")] public List<GraphNode> Nodes { get; set; } = new();
    [JsonPropertyName("links")] public List<GraphLink> Links { get; set; } = new();
    [JsonPropertyName("clusters")] public List<NetworkCluster> Clusters { get; set; } = new();
}

/// <summary>A cluster in the network analysis.</summary>
public class NetworkCluster
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("companyUids")] public List<string> CompanyUids { get; set; } = new();
    [JsonPropertyName("sharedPersons")] public List<string> SharedPersons { get; set; } = new();
}
