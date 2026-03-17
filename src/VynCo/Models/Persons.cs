using System.Text.Json.Serialization;

namespace VynCo.Models;

public class Person
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("fullName")] public string FullName { get; set; } = "";
    [JsonPropertyName("firstName")] public string? FirstName { get; set; }
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
    [JsonPropertyName("placeOfOrigin")] public string? PlaceOfOrigin { get; set; }
    [JsonPropertyName("residence")] public string? Residence { get; set; }
    [JsonPropertyName("roleCount")] public int RoleCount { get; set; }
    [JsonPropertyName("activeRoleCount")] public int ActiveRoleCount { get; set; }
}

public class PersonConnection
{
    [JsonPropertyName("personId")] public Guid PersonId { get; set; }
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("roleDescription")] public string RoleDescription { get; set; } = "";
    [JsonPropertyName("category")] public string Category { get; set; } = "";
    [JsonPropertyName("signatureAuthority")] public string SignatureAuthority { get; set; } = "";
    [JsonPropertyName("startDate")] public DateTime? StartDate { get; set; }
    [JsonPropertyName("endDate")] public DateTime? EndDate { get; set; }
    [JsonPropertyName("isActive")] public bool IsActive { get; set; }
}

public class BoardMember
{
    [JsonPropertyName("person")] public Person Person { get; set; } = new();
    [JsonPropertyName("role")] public PersonConnection Role { get; set; } = new();
}

/// <summary>Parameters for searching persons.</summary>
public class SearchPersonsParams
{
    public string? Query { get; set; }
    public int Limit { get; set; } = 50;
}
