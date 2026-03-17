using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Persons resource — search, get, roles, board members.</summary>
public class PersonsResource
{
    private readonly VynCoClient _client;
    internal PersonsResource(VynCoClient client) => _client = client;

    /// <summary>Search persons by name.</summary>
    public Task<List<Person>> SearchAsync(SearchPersonsParams? @params = null, CancellationToken ct = default)
    {
        var p = @params ?? new SearchPersonsParams();
        var query = $"?limit={p.Limit}";
        if (p.Query is not null) query += $"&q={Uri.EscapeDataString(p.Query)}";

        return _client.RequestListAsync<Person>(HttpMethod.Get, $"/v1/persons{query}", ct);
    }

    /// <summary>Get a person by ID.</summary>
    public Task<Person> GetAsync(Guid personId, CancellationToken ct = default)
        => _client.RequestAsync<Person>(HttpMethod.Get, $"/v1/persons/{personId}", ct);

    /// <summary>Get all roles for a person.</summary>
    public Task<List<PersonConnection>> GetRolesAsync(Guid personId, CancellationToken ct = default)
        => _client.RequestListAsync<PersonConnection>(HttpMethod.Get, $"/v1/persons/{personId}/roles", ct);

    /// <summary>Get board members for a company.</summary>
    public Task<List<BoardMember>> GetBoardMembersAsync(string companyUid, CancellationToken ct = default)
        => _client.RequestListAsync<BoardMember>(HttpMethod.Get, $"/v1/persons/board-members/{Uri.EscapeDataString(companyUid)}", ct);
}
