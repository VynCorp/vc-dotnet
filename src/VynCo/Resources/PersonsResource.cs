using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Persons resource — board members, search, get details.</summary>
public class PersonsResource
{
    private readonly VynCoClient _client;
    internal PersonsResource(VynCoClient client) => _client = client;

    /// <summary>Get board members for a company.</summary>
    public Task<List<BoardMember>> BoardMembersAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<BoardMember>(HttpMethod.Get, $"/v1/persons/board-members/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>Search for persons across all companies.</summary>
    public Task<PagedResponse<PersonSearchResult>> SearchAsync(PersonSearchParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.Q is not null) qs.Add($"q={Uri.EscapeDataString(@params.Q)}");
        if (@params?.Page is not null) qs.Add($"page={@params.Page}");
        if (@params?.PageSize is not null) qs.Add($"pageSize={@params.PageSize}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";

        return _client.RequestAsync<PagedResponse<PersonSearchResult>>(HttpMethod.Get, $"/v1/persons/search{query}", ct);
    }

    /// <summary>Get detailed person record by ID.</summary>
    public Task<PersonDetail> GetAsync(string id, CancellationToken ct = default)
        => _client.RequestAsync<PersonDetail>(HttpMethod.Get, $"/v1/persons/{Uri.EscapeDataString(id)}", ct);
}
