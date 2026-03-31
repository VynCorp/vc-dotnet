using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Persons resource — board members.</summary>
public class PersonsResource
{
    private readonly VynCoClient _client;
    internal PersonsResource(VynCoClient client) => _client = client;

    /// <summary>Get board members for a company.</summary>
    public Task<List<BoardMember>> BoardMembersAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<BoardMember>(HttpMethod.Get, $"/v1/persons/board-members/{Uri.EscapeDataString(uid)}", ct);
}
