using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Persons resource — board members, search, get details, network (v3.1+).</summary>
public class PersonsResource
{
    private readonly VynCoClient _client;
    internal PersonsResource(VynCoClient client) => _client = client;

    /// <summary>Get board members for a company (unpaginated — returns up to the server-side default).</summary>
    public Task<List<BoardMember>> BoardMembersAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<BoardMember>(HttpMethod.Get, $"/v1/persons/board-members/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>
    /// Get board members for a company with pagination (v3.1+).
    /// <paramref name="params"/>.Page is 1-indexed, <paramref name="params"/>.PageSize caps at 500
    /// (server default is 100). Essential for companies with large boards like UBS (1,100+ signatories).
    /// </summary>
    public Task<List<BoardMember>> BoardMembersPagedAsync(string uid, BoardMemberParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.Page is not null) qs.Add($"page={@params.Page}");
        if (@params?.PageSize is not null) qs.Add($"pageSize={@params.PageSize}");
        if (qs.Count == 0)
            return BoardMembersAsync(uid, ct);
        var query = "?" + string.Join("&", qs);
        return _client.RequestListAsync<BoardMember>(HttpMethod.Get, $"/v1/persons/board-members/{Uri.EscapeDataString(uid)}{query}", ct);
    }

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

    /// <summary>
    /// Get a person-centric network view (v3.1+). Returns the person's companies, co-directors
    /// (persons they share directorships with), and summary statistics. Useful for compliance
    /// investigations that start from a person rather than a company.
    /// </summary>
    public Task<PersonNetworkResponse> NetworkAsync(string id, CancellationToken ct = default)
        => _client.RequestAsync<PersonNetworkResponse>(HttpMethod.Get, $"/v1/persons/{Uri.EscapeDataString(id)}/network", ct);
}
