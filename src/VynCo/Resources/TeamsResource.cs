using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Teams resource — create and get team.</summary>
public class TeamsResource
{
    private readonly VynCoClient _client;
    internal TeamsResource(VynCoClient client) => _client = client;

    /// <summary>Create a new team.</summary>
    public Task<Team> CreateAsync(CreateTeamRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Team>(HttpMethod.Post, "/v1/teams", request, ct);

    /// <summary>Get the current authenticated team.</summary>
    public Task<Team> GetCurrentAsync(CancellationToken ct = default)
        => _client.RequestAsync<Team>(HttpMethod.Get, "/v1/teams/me", ct);
}
