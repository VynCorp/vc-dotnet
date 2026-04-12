using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>Query parameters for <c>companies.MediaAsync</c> (v3.1+).</summary>
public class MediaParams
{
    /// <summary><c>positive</c> | <c>neutral</c> | <c>negative</c>.</summary>
    public string? Sentiment { get; set; }
    public string? Since { get; set; }
    public long? Limit { get; set; }
}

/// <summary>A media/news item with optional sentiment analysis (v3.1+).</summary>
public class MediaItem
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("summary")] public string? Summary { get; set; }
    [JsonPropertyName("source")] public string? Source { get; set; }
    [JsonPropertyName("publishedAt")] public string? PublishedAt { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("sentimentScore")] public float? SentimentScore { get; set; }
    [JsonPropertyName("sentimentLabel")] public string? SentimentLabel { get; set; }
    [JsonPropertyName("topics")] public List<string>? Topics { get; set; }
    [JsonPropertyName("riskRelevance")] public float? RiskRelevance { get; set; }
}

/// <summary>Response containing a list of media items (v3.1+).</summary>
public class MediaResponse
{
    [JsonPropertyName("items")] public List<MediaItem> Items { get; set; } = new();
    [JsonPropertyName("total")] public long Total { get; set; }
}

/// <summary>Response from triggering LLM sentiment analysis on media items (v3.1+).</summary>
public class MediaAnalysisResponse
{
    [JsonPropertyName("analyzedCount")] public int AnalyzedCount { get; set; }
    [JsonPropertyName("message")] public string Message { get; set; } = "";
}
