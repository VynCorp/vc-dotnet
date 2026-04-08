using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A team.</summary>
public class Team
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("slug")] public string Slug { get; set; } = "";
    [JsonPropertyName("tier")] public string Tier { get; set; } = "";
    [JsonPropertyName("creditBalance")] public int CreditBalance { get; set; }
    [JsonPropertyName("monthlyCredits")] public long MonthlyCredits { get; set; }
}

/// <summary>Request body for creating a team.</summary>
public class CreateTeamRequest
{
    [JsonPropertyName("name")] public string? Name { get; set; }
}

/// <summary>A team member.</summary>
public class TeamMember
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("lastLoginAt")] public string? LastLoginAt { get; set; }
}

/// <summary>Request body for inviting a team member.</summary>
public class InviteMemberRequest
{
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("role")] public string? Role { get; set; }
}

/// <summary>An invitation to join a team.</summary>
public class Invitation
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("teamId")] public string TeamId { get; set; } = "";
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("token")] public string Token { get; set; } = "";
    [JsonPropertyName("status")] public string Status { get; set; } = "";
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("expiresAt")] public string ExpiresAt { get; set; } = "";
}

/// <summary>Request body for updating a member's role.</summary>
public class UpdateMemberRoleRequest
{
    [JsonPropertyName("role")] public string Role { get; set; } = "";
}

/// <summary>Request body for joining a team via invitation token.</summary>
public class JoinTeamRequest
{
    [JsonPropertyName("token")] public string Token { get; set; } = "";
}

/// <summary>Response from joining a team.</summary>
public class JoinTeamResponse
{
    [JsonPropertyName("teamId")] public string TeamId { get; set; } = "";
    [JsonPropertyName("teamName")] public string TeamName { get; set; } = "";
    [JsonPropertyName("role")] public string Role { get; set; } = "";
}

/// <summary>Billing summary for a team.</summary>
public class BillingSummary
{
    [JsonPropertyName("tier")] public string Tier { get; set; } = "";
    [JsonPropertyName("creditBalance")] public int CreditBalance { get; set; }
    [JsonPropertyName("monthlyCredits")] public long MonthlyCredits { get; set; }
    [JsonPropertyName("usedThisMonth")] public long UsedThisMonth { get; set; }
    [JsonPropertyName("members")] public List<MemberUsage> Members { get; set; } = new();
}

/// <summary>Credit usage by a single team member.</summary>
public class MemberUsage
{
    [JsonPropertyName("userId")] public string UserId { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("creditsUsed")] public long CreditsUsed { get; set; }
}
