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
    [JsonPropertyName("ownerEmail")] public string? OwnerEmail { get; set; }
    [JsonPropertyName("ownerName")] public string? OwnerName { get; set; }
}

public class TeamMember
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("isActive")] public bool IsActive { get; set; }
    [JsonPropertyName("invitedAt")] public DateTime? InvitedAt { get; set; }
    [JsonPropertyName("joinedAt")] public DateTime? JoinedAt { get; set; }
}

public class InviteMemberRequest
{
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("role")] public string Role { get; set; } = "Member";
    [JsonPropertyName("invitedBy")] public string? InvitedBy { get; set; }
}

public class UpdateMemberRoleRequest
{
    [JsonPropertyName("role")] public string Role { get; set; } = "";
}

public class BillingSummary
{
    [JsonPropertyName("members")] public List<MemberUsage> Members { get; set; } = new();
    [JsonPropertyName("totalCreditsUsed")] public int TotalCreditsUsed { get; set; }
    [JsonPropertyName("period")] public string Period { get; set; } = "";
}

public class MemberUsage
{
    [JsonPropertyName("memberId")] public Guid? MemberId { get; set; }
    [JsonPropertyName("memberName")] public string MemberName { get; set; } = "";
    [JsonPropertyName("memberEmail")] public string? MemberEmail { get; set; }
    [JsonPropertyName("creditsUsed")] public int CreditsUsed { get; set; }
    [JsonPropertyName("percentage")] public double Percentage { get; set; }
}
