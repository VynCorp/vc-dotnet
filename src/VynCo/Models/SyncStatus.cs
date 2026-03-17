using System.Text.Json.Serialization;

namespace VynCo.Models;

public class SyncStatusEntry
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("lastCompletedAt")] public DateTime? LastCompletedAt { get; set; }
    [JsonPropertyName("lastStartedAt")] public DateTime? LastStartedAt { get; set; }
    [JsonPropertyName("nextScheduledAt")] public DateTime? NextScheduledAt { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("lastError")] public string? LastError { get; set; }
    [JsonPropertyName("itemsProcessed")] public int ItemsProcessed { get; set; }
    [JsonPropertyName("itemsTotal")] public int ItemsTotal { get; set; }
}
