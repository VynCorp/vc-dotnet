using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Teams resource — create, get, manage members, billing summary.</summary>
public class TeamsResource
{
    private readonly VynCoClient _client;
    internal TeamsResource(VynCoClient client) => _client = client;

    /// <summary>Get the current authenticated team.</summary>
    public Task<Team> MeAsync(CancellationToken ct = default)
        => _client.RequestAsync<Team>(HttpMethod.Get, "/v1/teams/me", ct);

    /// <summary>Create a new team.</summary>
    public Task<Team> CreateAsync(CreateTeamRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Team>(HttpMethod.Post, "/v1/teams", request, ct);

    /// <summary>List all members of the current team.</summary>
    public Task<List<TeamMember>> MembersAsync(CancellationToken ct = default)
        => _client.RequestListAsync<TeamMember>(HttpMethod.Get, "/v1/teams/me/members", ct);

    /// <summary>Invite a user to the current team.</summary>
    public Task<Invitation> InviteMemberAsync(InviteMemberRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Invitation>(HttpMethod.Post, "/v1/teams/me/members", request, ct);

    /// <summary>Change a team member's role.</summary>
    public Task<TeamMember> UpdateMemberRoleAsync(string id, UpdateMemberRoleRequest request, CancellationToken ct = default)
        => _client.RequestAsync<TeamMember>(HttpMethod.Put, $"/v1/teams/me/members/{Uri.EscapeDataString(id)}", request, ct);

    /// <summary>Remove a member from the team.</summary>
    public Task RemoveMemberAsync(string id, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/teams/me/members/{Uri.EscapeDataString(id)}", ct);

    /// <summary>Get billing summary for the current team.</summary>
    public Task<BillingSummary> BillingSummaryAsync(CancellationToken ct = default)
        => _client.RequestAsync<BillingSummary>(HttpMethod.Get, "/v1/teams/me/billing-summary", ct);
}
