using System.Text.Json.Serialization;

namespace VynCo.Models;

public class Team
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("slug")] public string Slug { get; set; } = "";
    [JsonPropertyName("tier")] public string Tier { get; set; } = "";
    [JsonPropertyName("creditBalance")] public int CreditBalance { get; set; }
    [JsonPropertyName("monthlyCredits")] public int MonthlyCredits { get; set; }
    [JsonPropertyName("overageRate")] public decimal OverageRate { get; set; }
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updatedAt")] public DateTime? UpdatedAt { get; set; }
}

public class CreateTeamRequest
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("slug")] public string Slug { get; set; } = "";
}
