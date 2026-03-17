using System.Text.Json.Serialization;

namespace VynCo.Models;

public class ApiKey
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("keyPrefix")] public string KeyPrefix { get; set; } = "";
    [JsonPropertyName("keyHint")] public string KeyHint { get; set; } = "";
    [JsonPropertyName("permissions")] public List<string> Permissions { get; set; } = new();
    [JsonPropertyName("isActive")] public bool IsActive { get; set; }
    [JsonPropertyName("lastUsedAt")] public DateTime? LastUsedAt { get; set; }
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("expiresAt")] public DateTime? ExpiresAt { get; set; }
}

public class ApiKeyCreated
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("keyPrefix")] public string KeyPrefix { get; set; } = "";
    [JsonPropertyName("keyHint")] public string KeyHint { get; set; } = "";
    [JsonPropertyName("permissions")] public List<string> Permissions { get; set; } = new();
    [JsonPropertyName("isActive")] public bool IsActive { get; set; }
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("rawKey")] public string RawKey { get; set; } = "";
}

public class CreateApiKeyRequest
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("isTestKey")] public bool IsTestKey { get; set; }
    [JsonPropertyName("permissions")] public List<string>? Permissions { get; set; }
}
