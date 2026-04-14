using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A board member entry in a PDF profile response.</summary>
public class PdfBoardMember
{
    [JsonPropertyName("firstName")] public string? FirstName { get; set; }
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("signingAuthority")] public string? SigningAuthority { get; set; }
    [JsonPropertyName("since")] public string? Since { get; set; }
    [JsonPropertyName("until")] public string? Until { get; set; }
}

/// <summary>A company event in a PDF profile response.</summary>
public class PdfEvent
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("category")] public string Category { get; set; } = "";
    [JsonPropertyName("summary")] public string Summary { get; set; } = "";
    [JsonPropertyName("severity")] public string Severity { get; set; } = "";
    [JsonPropertyName("detectedAt")] public string DetectedAt { get; set; } = "";
    [JsonPropertyName("sourceDate")] public string? SourceDate { get; set; }
}

/// <summary>An auditor tenure entry in a PDF profile response.</summary>
public class PdfAuditorTenure
{
    [JsonPropertyName("auditorName")] public string AuditorName { get; set; } = "";
    [JsonPropertyName("appointedAt")] public string? AppointedAt { get; set; }
    [JsonPropertyName("resignedAt")] public string? ResignedAt { get; set; }
    [JsonPropertyName("tenureYears")] public double? TenureYears { get; set; }
    [JsonPropertyName("isCurrent")] public bool IsCurrent { get; set; }
}

/// <summary>Core company data within a PDF profile response.</summary>
public class PdfCompanyData
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("legalForm")] public string? LegalForm { get; set; }
    [JsonPropertyName("shareCapital")] public double? ShareCapital { get; set; }
    [JsonPropertyName("currency")] public string? Currency { get; set; }
    [JsonPropertyName("purpose")] public string? Purpose { get; set; }
    [JsonPropertyName("foundingDate")] public string? FoundingDate { get; set; }
    [JsonPropertyName("registrationDate")] public string? RegistrationDate { get; set; }
    [JsonPropertyName("legalSeat")] public string? LegalSeat { get; set; }
    [JsonPropertyName("municipality")] public string? Municipality { get; set; }
    [JsonPropertyName("addressStreet")] public string? AddressStreet { get; set; }
    [JsonPropertyName("addressHouseNumber")] public string? AddressHouseNumber { get; set; }
    [JsonPropertyName("addressZipCode")] public string? AddressZipCode { get; set; }
    [JsonPropertyName("addressCity")] public string? AddressCity { get; set; }
    [JsonPropertyName("website")] public string? Website { get; set; }
    [JsonPropertyName("industry")] public string? Industry { get; set; }
    [JsonPropertyName("subIndustry")] public string? SubIndustry { get; set; }
    [JsonPropertyName("employeeCount")] public int? EmployeeCount { get; set; }
    [JsonPropertyName("auditorName")] public string? AuditorName { get; set; }
    [JsonPropertyName("auditorCategory")] public string? AuditorCategory { get; set; }
    [JsonPropertyName("sanctionsHit")] public bool? SanctionsHit { get; set; }
    [JsonPropertyName("isFinmaRegulated")] public bool? IsFinmaRegulated { get; set; }
    [JsonPropertyName("oldNames")] public List<string>? OldNames { get; set; }
    [JsonPropertyName("translations")] public List<string>? Translations { get; set; }
}

/// <summary>Structured company profile data suitable for PDF rendering.</summary>
public class PdfProfileResponse
{
    [JsonPropertyName("company")] public PdfCompanyData Company { get; set; } = new();
    [JsonPropertyName("boardMembers")] public List<PdfBoardMember> BoardMembers { get; set; } = new();
    [JsonPropertyName("recentEvents")] public List<PdfEvent> RecentEvents { get; set; } = new();
    [JsonPropertyName("auditorHistory")] public List<PdfAuditorTenure> AuditorHistory { get; set; } = new();
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
}
