using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A saved search query that can be scheduled or linked to alerts.</summary>
public class SavedSearch
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("searchParams")] public JsonElement SearchParams { get; set; }
    [JsonPropertyName("isScheduled")] public bool IsScheduled { get; set; }
    [JsonPropertyName("scheduleFrequency")] public string? ScheduleFrequency { get; set; }
    [JsonPropertyName("lastRunAt")] public string? LastRunAt { get; set; }
    [JsonPropertyName("lastResultCount")] public int? LastResultCount { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("updatedAt")] public string UpdatedAt { get; set; } = "";
}

/// <summary>Request body for creating a saved search.</summary>
public class CreateSavedSearchRequest
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("searchParams")] public object SearchParams { get; set; } = new { };
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("isScheduled")] public bool IsScheduled { get; set; }
    [JsonPropertyName("scheduleFrequency")] public string? ScheduleFrequency { get; set; }
}

/// <summary>Request body for updating a saved search.</summary>
public class UpdateSavedSearchRequest
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("searchParams")] public object? SearchParams { get; set; }
    [JsonPropertyName("isScheduled")] public bool? IsScheduled { get; set; }
    [JsonPropertyName("scheduleFrequency")] public string? ScheduleFrequency { get; set; }
}
