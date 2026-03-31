using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A webhook subscription.</summary>
public class WebhookSubscription
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("url")] public string Url { get; set; } = "";
    [JsonPropertyName("description")] public string Description { get; set; } = "";
    [JsonPropertyName("eventFilters")] public List<string> EventFilters { get; set; } = new();
    [JsonPropertyName("companyFilters")] public List<string> CompanyFilters { get; set; } = new();
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("updatedAt")] public string UpdatedAt { get; set; } = "";
}

/// <summary>Request body for creating a webhook subscription.</summary>
public class CreateWebhookRequest
{
    [JsonPropertyName("url")] public string Url { get; set; } = "";
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("eventFilters")] public List<string>? EventFilters { get; set; }
    [JsonPropertyName("companyFilters")] public List<string>? CompanyFilters { get; set; }
}

/// <summary>Response from creating a webhook (includes signing secret).</summary>
public class CreateWebhookResponse
{
    [JsonPropertyName("webhook")] public WebhookSubscription Webhook { get; set; } = new();
    [JsonPropertyName("signingSecret")] public string SigningSecret { get; set; } = "";
}

/// <summary>Request body for updating a webhook subscription.</summary>
public class UpdateWebhookRequest
{
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("eventFilters")] public List<string>? EventFilters { get; set; }
    [JsonPropertyName("companyFilters")] public List<string>? CompanyFilters { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
}

/// <summary>Response from testing a webhook delivery.</summary>
public class TestDeliveryResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("httpStatus")] public int? HttpStatus { get; set; }
    [JsonPropertyName("error")] public string? Error { get; set; }
}

/// <summary>A webhook delivery record.</summary>
public class WebhookDelivery
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("eventId")] public string EventId { get; set; } = "";
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("attempt")] public int Attempt { get; set; }
    [JsonPropertyName("httpStatus")] public int? HttpStatus { get; set; }
    [JsonPropertyName("errorMessage")] public string? ErrorMessage { get; set; }
    [JsonPropertyName("deliveredAt")] public string? DeliveredAt { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}
