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
}
