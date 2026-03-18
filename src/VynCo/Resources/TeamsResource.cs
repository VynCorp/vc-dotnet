using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Teams resource — create, get, manage members, billing summary.</summary>
public class TeamsResource
{
    private readonly VynCoClient _client;
    internal TeamsResource(VynCoClient client) => _client = client;

    /// <summary>Create a new team.</summary>
    public Task<Team> CreateAsync(CreateTeamRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Team>(HttpMethod.Post, "/api/v1/teams", request, ct);

    /// <summary>Get the current authenticated team.</summary>
    public Task<Team> GetCurrentAsync(CancellationToken ct = default)
        => _client.RequestAsync<Team>(HttpMethod.Get, "/api/v1/teams/me", ct);

    /// <summary>List all members of the current team.</summary>
    public Task<List<TeamMember>> ListMembersAsync(CancellationToken ct = default)
        => _client.RequestListAsync<TeamMember>(HttpMethod.Get, "/api/v1/teams/me/members", ct);

    /// <summary>Invite a user to the current team. Requires Owner or Admin role.</summary>
    public Task<TeamMember> InviteMemberAsync(InviteMemberRequest request, CancellationToken ct = default)
        => _client.RequestAsync<TeamMember>(HttpMethod.Post, "/api/v1/teams/me/members", request, ct);

    /// <summary>Change a team member's role. Requires Owner or Admin.</summary>
    public Task<TeamMember> UpdateMemberRoleAsync(Guid memberId, UpdateMemberRoleRequest request, CancellationToken ct = default)
        => _client.RequestAsync<TeamMember>(HttpMethod.Put, $"/api/v1/teams/me/members/{memberId}", request, ct);

    /// <summary>Remove a member from the team. Requires Owner or Admin.</summary>
    public Task RemoveMemberAsync(Guid memberId, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/api/v1/teams/me/members/{memberId}", ct);

    /// <summary>Get credit usage breakdown per member for the current billing period.</summary>
    public Task<BillingSummary> BillingSummaryAsync(CancellationToken ct = default)
        => _client.RequestAsync<BillingSummary>(HttpMethod.Get, "/api/v1/teams/me/billing-summary", ct);
}
