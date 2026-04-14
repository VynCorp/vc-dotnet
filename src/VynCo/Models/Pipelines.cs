using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A sales/tracking pipeline.</summary>
public class Pipeline
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("teamId")] public string TeamId { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("stages")] public List<string> Stages { get; set; } = new();
    [JsonPropertyName("createdAt")] public long CreatedAt { get; set; }
}

/// <summary>An entry (company) within a pipeline stage.</summary>
public class PipelineEntry
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("pipelineId")] public string PipelineId { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("stage")] public string Stage { get; set; } = "";
    [JsonPropertyName("assignedToUserId")] public string? AssignedToUserId { get; set; }
    [JsonPropertyName("assignedToName")] public string? AssignedToName { get; set; }
    [JsonPropertyName("tier")] public int Tier { get; set; } = 3;
    [JsonPropertyName("score")] public double? Score { get; set; }
    [JsonPropertyName("notes")] public string? Notes { get; set; }
    [JsonPropertyName("createdAt")] public long CreatedAt { get; set; }
    [JsonPropertyName("updatedAt")] public long UpdatedAt { get; set; }
}

/// <summary>A pipeline with its entries loaded.</summary>
public class PipelineWithEntries
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("teamId")] public string TeamId { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("stages")] public List<string> Stages { get; set; } = new();
    [JsonPropertyName("createdAt")] public long CreatedAt { get; set; }
    [JsonPropertyName("entries")] public List<PipelineEntry> Entries { get; set; } = new();
    [JsonPropertyName("totalEntries")] public int TotalEntries { get; set; }
}

/// <summary>Aggregate statistics for a pipeline.</summary>
public class PipelineStats
{
    [JsonPropertyName("byStage")] public Dictionary<string, int> ByStage { get; set; } = new();
    [JsonPropertyName("byTier")] public Dictionary<string, int> ByTier { get; set; } = new();
    [JsonPropertyName("total")] public int Total { get; set; }
}

/// <summary>Request body for creating a pipeline.</summary>
public class CreatePipelineRequest
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("stages")] public List<string>? Stages { get; set; }
}

/// <summary>Request body for adding an entry to a pipeline.</summary>
public class AddEntryRequest
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("stage")] public string? Stage { get; set; }
    [JsonPropertyName("tier")] public int? Tier { get; set; }
    [JsonPropertyName("assignedToUserId")] public string? AssignedToUserId { get; set; }
}

/// <summary>Request body for updating a pipeline entry.</summary>
public class UpdateEntryRequest
{
    [JsonPropertyName("stage")] public string? Stage { get; set; }
    [JsonPropertyName("tier")] public int? Tier { get; set; }
    [JsonPropertyName("assignedToUserId")] public string? AssignedToUserId { get; set; }
    [JsonPropertyName("notes")] public string? Notes { get; set; }
}
