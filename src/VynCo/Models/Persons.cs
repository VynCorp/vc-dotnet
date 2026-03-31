using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A board member of a company.</summary>
public class BoardMember
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("firstName")] public string? FirstName { get; set; }
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("roleCategory")] public string RoleCategory { get; set; } = "";
    [JsonPropertyName("origin")] public string? Origin { get; set; }
    [JsonPropertyName("residence")] public string? Residence { get; set; }
    [JsonPropertyName("signingAuthority")] public string? SigningAuthority { get; set; }
    [JsonPropertyName("since")] public string? Since { get; set; }
}
