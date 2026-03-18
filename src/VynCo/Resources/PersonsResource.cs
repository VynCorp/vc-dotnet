using System.Text.Json;
using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Persons resource — list, get, roles, board members, connections, network stats.</summary>
public class PersonsResource
{
    private readonly VynCoClient _client;
    internal PersonsResource(VynCoClient client) => _client = client;

    /// <summary>List persons with pagination and optional search filter.</summary>
    public Task<List<Person>> ListAsync(ListPersonsParams? @params = null, CancellationToken ct = default)
    {
        var p = @params ?? new ListPersonsParams();
        var query = $"?page={p.Page}&pageSize={p.PageSize}";
        if (p.Search is not null) query += $"&search={Uri.EscapeDataString(p.Search)}";

        return _client.RequestListAsync<Person>(HttpMethod.Get, $"/api/v1/persons{query}", ct);
    }

    /// <summary>Get a person by ID.</summary>
    public Task<Person> GetAsync(Guid personId, CancellationToken ct = default)
        => _client.RequestAsync<Person>(HttpMethod.Get, $"/api/v1/persons/{personId}", ct);

    /// <summary>Get all roles for a person.</summary>
    public Task<List<PersonConnection>> GetRolesAsync(Guid personId, CancellationToken ct = default)
        => _client.RequestListAsync<PersonConnection>(HttpMethod.Get, $"/api/v1/persons/{personId}/roles", ct);

    /// <summary>Get the network of companies connected to a person. Cost: 10 credits.</summary>
    public Task<List<JsonElement>> GetConnectionsAsync(Guid personId, CancellationToken ct = default)
        => _client.RequestListAsync<JsonElement>(HttpMethod.Get, $"/api/v1/persons/{personId}/connections", ct);

    /// <summary>Get board members for a company.</summary>
    public Task<List<Person>> GetBoardMembersAsync(string companyUid, CancellationToken ct = default)
        => _client.RequestListAsync<Person>(HttpMethod.Get, $"/api/v1/persons/board-members/{Uri.EscapeDataString(companyUid)}", ct);

    /// <summary>Get aggregate statistics on the person network graph. Cost: 3 credits.</summary>
    public Task<JsonElement> NetworkStatsAsync(CancellationToken ct = default)
        => _client.RequestAsync<JsonElement>(HttpMethod.Get, "/api/v1/persons/network-stats", ct);
}
