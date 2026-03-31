using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Request body for creating a dossier.</summary>
public class CreateDossierRequest
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("level")] public string? Level { get; set; }
}

/// <summary>A managed company dossier.</summary>
public class Dossier
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("userId")] public string UserId { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("level")] public string Level { get; set; } = "";
    [JsonPropertyName("content")] public string Content { get; set; } = "";
    [JsonPropertyName("sources")] public List<string> Sources { get; set; } = new();
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}

/// <summary>Summary of a dossier (used in list responses).</summary>
public class DossierSummary
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("level")] public string Level { get; set; } = "";
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}
