using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A board member of a company.</summary>
public class BoardMember
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("firstName")] public string? FirstName { get; set; }
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("roleCategory")] public string RoleCategory { get; set; } = "";
    [JsonPropertyName("origin")] public string? Origin { get; set; }
    [JsonPropertyName("residence")] public string? Residence { get; set; }
    [JsonPropertyName("signingAuthority")] public string? SigningAuthority { get; set; }
    [JsonPropertyName("since")] public string? Since { get; set; }
    // --- Enrichment provenance (v3.1+) ---
    [JsonPropertyName("roleSource")] public string? RoleSource { get; set; }
    [JsonPropertyName("roleConfidence")] public double? RoleConfidence { get; set; }
    [JsonPropertyName("roleInferredAt")] public string? RoleInferredAt { get; set; }
}

/// <summary>Query parameters for paginated board member listing (v3.1+).</summary>
public class BoardMemberParams
{
    public long? Page { get; set; }
    public long? PageSize { get; set; }
}

/// <summary>Query parameters for searching persons.</summary>
public class PersonSearchParams
{
    public string? Q { get; set; }
    public long? Page { get; set; }
    public long? PageSize { get; set; }
}

/// <summary>Person search result summary.</summary>
public class PersonSearchResult
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("fullName")] public string FullName { get; set; } = "";
    [JsonPropertyName("firstName")] public string? FirstName { get; set; }
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
    [JsonPropertyName("placeOfOrigin")] public string? PlaceOfOrigin { get; set; }
    [JsonPropertyName("nationality")] public string? Nationality { get; set; }
    [JsonPropertyName("roleCount")] public long? RoleCount { get; set; }
}

/// <summary>Detailed person record.</summary>
public class PersonDetail
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("fullName")] public string FullName { get; set; } = "";
    [JsonPropertyName("firstName")] public string? FirstName { get; set; }
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
    [JsonPropertyName("placeOfOrigin")] public string? PlaceOfOrigin { get; set; }
    [JsonPropertyName("residence")] public string? Residence { get; set; }
    [JsonPropertyName("nationality")] public string? Nationality { get; set; }
    [JsonPropertyName("roles")] public List<PersonRoleDetail> Roles { get; set; } = new();
}

/// <summary>A person's role at a company.</summary>
public class PersonRoleDetail
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string? CompanyName { get; set; }
    [JsonPropertyName("roleFunction")] public string RoleFunction { get; set; } = "";
    [JsonPropertyName("roleCategory")] public string RoleCategory { get; set; } = "";
    [JsonPropertyName("signingAuthority")] public string? SigningAuthority { get; set; }
    [JsonPropertyName("startDate")] public string? StartDate { get; set; }
    [JsonPropertyName("endDate")] public string? EndDate { get; set; }
    [JsonPropertyName("changeAction")] public string? ChangeAction { get; set; }
    [JsonPropertyName("isCurrent")] public bool? IsCurrent { get; set; }
    // --- Enrichment provenance (v3.1+) ---
    [JsonPropertyName("roleSource")] public string? RoleSource { get; set; }
    [JsonPropertyName("roleConfidence")] public double? RoleConfidence { get; set; }
    [JsonPropertyName("roleInferredAt")] public string? RoleInferredAt { get; set; }
}

// ---------------------------------------------------------------------------
// Person network (v3.1+)
// ---------------------------------------------------------------------------

/// <summary>Summary of a person in a network response (v3.1+).</summary>
public class NetworkPerson
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("fullName")] public string FullName { get; set; } = "";
    [JsonPropertyName("firstName")] public string? FirstName { get; set; }
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
}

/// <summary>A company in a person's network (v3.1+).</summary>
public class NetworkCompany
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("roleCategory")] public string RoleCategory { get; set; } = "";
    [JsonPropertyName("isCurrent")] public bool? IsCurrent { get; set; }
    [JsonPropertyName("since")] public string? Since { get; set; }
    [JsonPropertyName("until")] public string? Until { get; set; }
    // --- Enrichment provenance (v3.1+) ---
    [JsonPropertyName("roleSource")] public string? RoleSource { get; set; }
    [JsonPropertyName("roleConfidence")] public double? RoleConfidence { get; set; }
    [JsonPropertyName("roleInferredAt")] public string? RoleInferredAt { get; set; }
}

/// <summary>A company shared between a person and a co-director (v3.1+).</summary>
public class CoDirectorCompany
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
}

/// <summary>A person who shares company directorships with the primary person (v3.1+).</summary>
public class CoDirector
{
    [JsonPropertyName("personId")] public string PersonId { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("sharedCompanies")] public long SharedCompanies { get; set; }
    [JsonPropertyName("companies")] public List<CoDirectorCompany> Companies { get; set; } = new();
}

/// <summary>Aggregate statistics for a person's network (v3.1+).</summary>
public class NetworkStats
{
    [JsonPropertyName("totalCompanies")] public long TotalCompanies { get; set; }
    [JsonPropertyName("activeRoles")] public long ActiveRoles { get; set; }
    [JsonPropertyName("coDirectorCount")] public long CoDirectorCount { get; set; }
}

/// <summary>Response for a person-centric network view (v3.1+).</summary>
public class PersonNetworkResponse
{
    [JsonPropertyName("person")] public NetworkPerson Person { get; set; } = new();
    [JsonPropertyName("companies")] public List<NetworkCompany> Companies { get; set; } = new();
    [JsonPropertyName("coDirectors")] public List<CoDirector> CoDirectors { get; set; } = new();
    [JsonPropertyName("stats")] public NetworkStats Stats { get; set; } = new();
}
