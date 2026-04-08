using System.Globalization;
using VynCo.Models;

namespace VynCo.Resources;

/// <summary>Companies resource — list, get, full, count, events, statistics, compare, news, reports, relationships, hierarchy, classification, fingerprint, structure, acquisitions, nearby, notes, tags, excel export.</summary>
public class CompaniesResource
{
    private readonly VynCoClient _client;
    internal CompaniesResource(VynCoClient client) => _client = client;

    /// <summary>List companies with pagination and optional filtering.</summary>
    public Task<PagedResponse<Company>> ListAsync(CompanyListParams? @params = null, CancellationToken ct = default)
    {
        var qs = new List<string>();
        if (@params?.Search is not null) qs.Add($"search={Uri.EscapeDataString(@params.Search)}");
        if (@params?.Canton is not null) qs.Add($"canton={Uri.EscapeDataString(@params.Canton)}");
        if (@params?.ChangedSince is not null) qs.Add($"changedSince={Uri.EscapeDataString(@params.ChangedSince)}");
        if (@params?.Status is not null) qs.Add($"status={Uri.EscapeDataString(@params.Status)}");
        if (@params?.LegalForm is not null) qs.Add($"legalForm={Uri.EscapeDataString(@params.LegalForm)}");
        if (@params?.CapitalMin is not null) qs.Add($"capitalMin={@params.CapitalMin.Value.ToString(CultureInfo.InvariantCulture)}");
        if (@params?.CapitalMax is not null) qs.Add($"capitalMax={@params.CapitalMax.Value.ToString(CultureInfo.InvariantCulture)}");
        if (@params?.AuditorCategory is not null) qs.Add($"auditorCategory={Uri.EscapeDataString(@params.AuditorCategory)}");
        if (@params?.SortBy is not null) qs.Add($"sortBy={Uri.EscapeDataString(@params.SortBy)}");
        if (@params?.SortDesc is not null) qs.Add($"sortDesc={@params.SortDesc.Value.ToString().ToLowerInvariant()}");
        if (@params?.Page is not null) qs.Add($"page={@params.Page}");
        if (@params?.PageSize is not null) qs.Add($"pageSize={@params.PageSize}");
        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : "";

        return _client.RequestAsync<PagedResponse<Company>>(HttpMethod.Get, $"/v1/companies{query}", ct);
    }

    /// <summary>Get a single company by its Swiss UID.</summary>
    public Task<Company> GetAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<Company>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}", ct);

    /// <summary>Get full company data including persons, recent changes, and relationships.</summary>
    public Task<CompanyFullResponse> GetFullAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<CompanyFullResponse>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/full", ct);

    /// <summary>Get total company count.</summary>
    public Task<CompanyCount> CountAsync(CancellationToken ct = default)
        => _client.RequestAsync<CompanyCount>(HttpMethod.Get, "/v1/companies/count", ct);

    /// <summary>Get events for a company.</summary>
    public Task<EventListResponse> EventsAsync(string uid, int? limit = null, CancellationToken ct = default)
    {
        var query = limit.HasValue ? $"?limit={limit.Value}" : "";
        return _client.RequestAsync<EventListResponse>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/events{query}", ct);
    }

    /// <summary>Get aggregate statistics across the registry.</summary>
    public Task<CompanyStatistics> StatisticsAsync(CancellationToken ct = default)
        => _client.RequestAsync<CompanyStatistics>(HttpMethod.Get, "/v1/companies/statistics", ct);

    /// <summary>Compare two or more companies side-by-side.</summary>
    public Task<CompareResponse> CompareAsync(CompareRequest request, CancellationToken ct = default)
        => _client.RequestAsync<CompareResponse>(HttpMethod.Post, "/v1/companies/compare", request, ct);

    /// <summary>Get news for a company.</summary>
    public Task<List<NewsItem>> NewsAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<NewsItem>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/news", ct);

    /// <summary>Get financial reports for a company.</summary>
    public Task<List<CompanyReport>> ReportsAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<CompanyReport>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/reports", ct);

    /// <summary>Get relationships for a company.</summary>
    public Task<List<Relationship>> RelationshipsAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<Relationship>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/relationships", ct);

    /// <summary>Get the corporate hierarchy for a company.</summary>
    public Task<HierarchyResponse> HierarchyAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<HierarchyResponse>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/hierarchy", ct);

    /// <summary>Get the industry classification for a company.</summary>
    public Task<Classification> ClassificationAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<Classification>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/classification", ct);

    /// <summary>Get the data fingerprint for a company.</summary>
    public Task<Fingerprint> FingerprintAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<Fingerprint>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/fingerprint", ct);

    /// <summary>Get the corporate structure for a company.</summary>
    public Task<CorporateStructure> StructureAsync(string uid, CancellationToken ct = default)
        => _client.RequestAsync<CorporateStructure>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/structure", ct);

    /// <summary>Get acquisitions for a company.</summary>
    public Task<List<Acquisition>> AcquisitionsAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<Acquisition>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/acquisitions", ct);

    /// <summary>Find companies near a geographic location.</summary>
    public Task<List<NearbyCompany>> NearbyAsync(NearbyParams @params, CancellationToken ct = default)
    {
        var qs = new List<string> { $"lat={@params.Lat.ToString(CultureInfo.InvariantCulture)}", $"lng={@params.Lng.ToString(CultureInfo.InvariantCulture)}" };
        if (@params.RadiusKm.HasValue) qs.Add($"radiusKm={@params.RadiusKm.Value.ToString(CultureInfo.InvariantCulture)}");
        if (@params.Limit.HasValue) qs.Add($"limit={@params.Limit.Value}");
        var query = "?" + string.Join("&", qs);

        return _client.RequestListAsync<NearbyCompany>(HttpMethod.Get, $"/v1/companies/nearby{query}", ct);
    }

    /// <summary>Get notes for a company.</summary>
    public Task<List<Note>> NotesAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<Note>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/notes", ct);

    /// <summary>Create a note on a company.</summary>
    public Task<Note> CreateNoteAsync(string uid, CreateNoteRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Note>(HttpMethod.Post, $"/v1/companies/{Uri.EscapeDataString(uid)}/notes", request, ct);

    /// <summary>Update a note on a company.</summary>
    public Task<Note> UpdateNoteAsync(string uid, string noteId, UpdateNoteRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Note>(HttpMethod.Put, $"/v1/companies/{Uri.EscapeDataString(uid)}/notes/{Uri.EscapeDataString(noteId)}", request, ct);

    /// <summary>Delete a note from a company.</summary>
    public Task DeleteNoteAsync(string uid, string noteId, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/companies/{Uri.EscapeDataString(uid)}/notes/{Uri.EscapeDataString(noteId)}", ct);

    /// <summary>Get tags for a company.</summary>
    public Task<List<Tag>> TagsAsync(string uid, CancellationToken ct = default)
        => _client.RequestListAsync<Tag>(HttpMethod.Get, $"/v1/companies/{Uri.EscapeDataString(uid)}/tags", ct);

    /// <summary>Create a tag on a company.</summary>
    public Task<Tag> CreateTagAsync(string uid, CreateTagRequest request, CancellationToken ct = default)
        => _client.RequestAsync<Tag>(HttpMethod.Post, $"/v1/companies/{Uri.EscapeDataString(uid)}/tags", request, ct);

    /// <summary>Delete a tag from a company.</summary>
    public Task DeleteTagAsync(string uid, string tagId, CancellationToken ct = default)
        => _client.RequestVoidAsync(HttpMethod.Delete, $"/v1/companies/{Uri.EscapeDataString(uid)}/tags/{Uri.EscapeDataString(tagId)}", ct);

    /// <summary>Get all tags across all companies.</summary>
    public Task<List<TagSummary>> AllTagsAsync(CancellationToken ct = default)
        => _client.RequestListAsync<TagSummary>(HttpMethod.Get, "/v1/tags", ct);

    /// <summary>Export companies to Excel.</summary>
    public Task<ExportFile> ExportExcelAsync(ExcelExportRequest request, CancellationToken ct = default)
        => _client.RequestBytesAsync("/v1/companies/export/excel", request, ct);
}
