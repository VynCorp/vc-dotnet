using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Request body for creating a data export.</summary>
public class CreateExportRequest
{
    [JsonPropertyName("format")] public string? Format { get; set; }
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("changedSince")] public string? ChangedSince { get; set; }
    [JsonPropertyName("industry")] public string? Industry { get; set; }
    [JsonPropertyName("maxRows")] public long? MaxRows { get; set; }
}

/// <summary>An export job record.</summary>
public class ExportJob
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("format")] public string Format { get; set; } = "";
    [JsonPropertyName("totalRows")] public int? TotalRows { get; set; }
    [JsonPropertyName("fileSizeBytes")] public long? FileSizeBytes { get; set; }
    [JsonPropertyName("errorMessage")] public string? ErrorMessage { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("completedAt")] public string? CompletedAt { get; set; }
    [JsonPropertyName("expiresAt")] public string? ExpiresAt { get; set; }
}

/// <summary>Export download response (job metadata + optional data).</summary>
public class ExportDownload
{
    [JsonPropertyName("job")] public ExportJob Job { get; set; } = new();
    [JsonPropertyName("data")] public string? Data { get; set; }
}

/// <summary>Downloaded export file (raw bytes + metadata).</summary>
public class ExportFile
{
    public VynCoResponseHeaders Headers { get; set; } = new();
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "";
    public string Filename { get; set; } = "";
}
