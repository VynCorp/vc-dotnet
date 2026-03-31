using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>An API key.</summary>
public class ApiKey
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("prefix")] public string Prefix { get; set; } = "";
    [JsonPropertyName("environment")] public string Environment { get; set; } = "";
    [JsonPropertyName("scopes")] public List<string> Scopes { get; set; } = new();
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("expiresAt")] public string? ExpiresAt { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("lastUsedAt")] public string? LastUsedAt { get; set; }
}

/// <summary>An API key as returned on creation (includes the raw key).</summary>
public class ApiKeyCreated
{
    [JsonPropertyName("key")] public string Key { get; set; } = "";
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("prefix")] public string Prefix { get; set; } = "";
    [JsonPropertyName("environment")] public string Environment { get; set; } = "";
    [JsonPropertyName("scopes")] public List<string> Scopes { get; set; } = new();
    [JsonPropertyName("expiresAt")] public string? ExpiresAt { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("warning")] public string Warning { get; set; } = "";
}

/// <summary>Request body for creating an API key.</summary>
public class CreateApiKeyRequest
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("environment")] public string? Environment { get; set; }
    [JsonPropertyName("scopes")] public List<string>? Scopes { get; set; }
}
