using System.Text.Json;
using System.Text.Json.Serialization;

namespace VynCo.Models;

/// <summary>A Swiss company record.</summary>
public class Company
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
    [JsonPropertyName("deletionDate")] public string? DeletionDate { get; set; }
    [JsonPropertyName("legalSeat")] public string? LegalSeat { get; set; }
    [JsonPropertyName("municipality")] public string? Municipality { get; set; }
    [JsonPropertyName("dataSource")] public string? DataSource { get; set; }
    [JsonPropertyName("enrichmentLevel")] public string? EnrichmentLevel { get; set; }
    [JsonPropertyName("addressStreet")] public string? AddressStreet { get; set; }
    [JsonPropertyName("addressHouseNumber")] public string? AddressHouseNumber { get; set; }
    [JsonPropertyName("addressZipCode")] public string? AddressZipCode { get; set; }
    [JsonPropertyName("addressCity")] public string? AddressCity { get; set; }
    [JsonPropertyName("addressCanton")] public string? AddressCanton { get; set; }
    [JsonPropertyName("website")] public string? Website { get; set; }
    [JsonPropertyName("industry")] public string? Industry { get; set; }
    [JsonPropertyName("subIndustry")] public string? SubIndustry { get; set; }
    [JsonPropertyName("employeeCount")] public int? EmployeeCount { get; set; }
    [JsonPropertyName("auditorName")] public string? AuditorName { get; set; }
    [JsonPropertyName("auditorCategory")] public string? AuditorCategory { get; set; }
    [JsonPropertyName("latitude")] public double? Latitude { get; set; }
    [JsonPropertyName("longitude")] public double? Longitude { get; set; }
    [JsonPropertyName("geoPrecision")] public string? GeoPrecision { get; set; }
    [JsonPropertyName("nogaCode")] public string? NogaCode { get; set; }
    [JsonPropertyName("sanctionsHit")] public bool? SanctionsHit { get; set; }
    [JsonPropertyName("lastScreenedAt")] public string? LastScreenedAt { get; set; }
    [JsonPropertyName("isFinmaRegulated")] public bool? IsFinmaRegulated { get; set; }
    [JsonPropertyName("ehraid")] public long? Ehraid { get; set; }
    [JsonPropertyName("chid")] public string? Chid { get; set; }
    [JsonPropertyName("cantonalExcerptUrl")] public string? CantonalExcerptUrl { get; set; }
    [JsonPropertyName("oldNames")] public List<string>? OldNames { get; set; }
    [JsonPropertyName("translations")] public List<string>? Translations { get; set; }
    [JsonPropertyName("updatedAt")] public string? UpdatedAt { get; set; }

    // --- Enrichment provenance (v3.1+) ---
    /// <summary>GLEIF-sourced direct parent LEI.</summary>
    [JsonPropertyName("directParentLei")] public string? DirectParentLei { get; set; }
    /// <summary>GLEIF-sourced ultimate parent LEI. Non-Swiss parents appear as <c>LEI:{20-char-lei}</c>.</summary>
    [JsonPropertyName("ultimateParentLei")] public string? UltimateParentLei { get; set; }
    /// <summary>Cached name of the ultimate parent.</summary>
    [JsonPropertyName("ultimateParentName")] public string? UltimateParentName { get; set; }
    /// <summary>Timestamp when GLEIF parent enrichment last ran.</summary>
    [JsonPropertyName("gleifParentEnrichedAt")] public string? GleifParentEnrichedAt { get; set; }
    /// <summary>Source of the industry classification: <c>zefix</c>, <c>keyword_match</c>, or <c>llm</c>.</summary>
    [JsonPropertyName("industrySource")] public string? IndustrySource { get; set; }
    /// <summary>Confidence (0–1) for LLM-classified industries.</summary>
    [JsonPropertyName("industryConfidence")] public double? IndustryConfidence { get; set; }
    /// <summary>Timestamp when the industry classification was last computed.</summary>
    [JsonPropertyName("industryClassifiedAt")] public string? IndustryClassifiedAt { get; set; }

    // --- External identifiers ---
    /// <summary>Legal Entity Identifier.</summary>
    [JsonPropertyName("lei")] public string? Lei { get; set; }
    /// <summary>D-U-N-S Number.</summary>
    [JsonPropertyName("duns")] public string? Duns { get; set; }
    /// <summary>International Securities Identification Number.</summary>
    [JsonPropertyName("isin")] public string? Isin { get; set; }
}

public class CompanyCount
{
    [JsonPropertyName("count")] public long Count { get; set; }
}

public class CompanyStatistics
{
    [JsonPropertyName("total")] public long Total { get; set; }
    [JsonPropertyName("byStatus")] public Dictionary<string, long> ByStatus { get; set; } = new();
    [JsonPropertyName("byCanton")] public Dictionary<string, long> ByCanton { get; set; } = new();
    [JsonPropertyName("byLegalForm")] public Dictionary<string, long> ByLegalForm { get; set; } = new();
}

/// <summary>Query parameters for listing companies.</summary>
public class CompanyListParams
{
    public string? Search { get; set; }
    public string? Canton { get; set; }
    public string? ChangedSince { get; set; }
    public string? Status { get; set; }
    public string? LegalForm { get; set; }
    public double? CapitalMin { get; set; }
    public double? CapitalMax { get; set; }
    public string? AuditorCategory { get; set; }
    public string? SortBy { get; set; }
    public bool? SortDesc { get; set; }
    public long? Page { get; set; }
    public long? PageSize { get; set; }
    public string? Lei { get; set; }
    public string? Duns { get; set; }
    public string? Isin { get; set; }
}

/// <summary>Response wrapper for event listing.</summary>
public class EventListResponse
{
    [JsonPropertyName("events")] public List<CompanyEvent> Events { get; set; } = new();
    [JsonPropertyName("count")] public long Count { get; set; }
}

/// <summary>A CloudEvent-style company event.</summary>
public class CompanyEvent
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("ceType")] public string CeType { get; set; } = "";
    [JsonPropertyName("ceSource")] public string CeSource { get; set; } = "";
    [JsonPropertyName("ceTime")] public string CeTime { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("category")] public string Category { get; set; } = "";
    [JsonPropertyName("severity")] public string Severity { get; set; } = "";
    [JsonPropertyName("summary")] public string Summary { get; set; } = "";
    [JsonPropertyName("detailJson")] public JsonElement DetailJson { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}

/// <summary>Request body for comparing companies.</summary>
public class CompareRequest
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
}

/// <summary>Company comparison response.</summary>
public class CompareResponse
{
    [JsonPropertyName("uids")] public List<string> Uids { get; set; } = new();
    [JsonPropertyName("names")] public List<string> Names { get; set; } = new();
    [JsonPropertyName("dimensions")] public List<ComparisonDimension> Dimensions { get; set; } = new();
}

/// <summary>A single dimension in a company comparison.</summary>
public class ComparisonDimension
{
    [JsonPropertyName("field")] public string Field { get; set; } = "";
    [JsonPropertyName("label")] public string Label { get; set; } = "";
    [JsonPropertyName("values")] public List<string?> Values { get; set; } = new();
}

/// <summary>A news item for a company.</summary>
public class NewsItem
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("summary")] public string? Summary { get; set; }
    [JsonPropertyName("source")] public string? Source { get; set; }
    [JsonPropertyName("sourceType")] public string SourceType { get; set; } = "";
    [JsonPropertyName("publishedAt")] public string PublishedAt { get; set; } = "";
    [JsonPropertyName("sourceUrl")] public string? Url { get; set; }
}

/// <summary>A financial report for a company.</summary>
public class CompanyReport
{
    [JsonPropertyName("reportType")] public string ReportType { get; set; } = "";
    [JsonPropertyName("fiscalYear")] public int? FiscalYear { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = "";
    [JsonPropertyName("sourceUrl")] public string? SourceUrl { get; set; }
    [JsonPropertyName("publicationDate")] public string PublicationDate { get; set; } = "";
}

/// <summary>A relationship between two companies.</summary>
public class Relationship
{
    [JsonPropertyName("relatedUid")] public string RelatedUid { get; set; } = "";
    [JsonPropertyName("relatedName")] public string RelatedName { get; set; } = "";
    [JsonPropertyName("relationshipType")] public string RelationshipType { get; set; } = "";
    [JsonPropertyName("sharedPersons")] public List<string> SharedPersons { get; set; } = new();
}

/// <summary>A company entity in a hierarchy response.</summary>
public class HierarchyEntity
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("confidence")] public string? Confidence { get; set; }
    [JsonPropertyName("sharedPersonCount")] public long? SharedPersonCount { get; set; }
    [JsonPropertyName("sharedPersons")] public List<string>? SharedPersons { get; set; }
}

/// <summary>Corporate hierarchy response.</summary>
public class HierarchyResponse
{
    [JsonPropertyName("parent")] public HierarchyEntity? Parent { get; set; }
    [JsonPropertyName("subsidiaries")] public List<HierarchyEntity> Subsidiaries { get; set; } = new();
    [JsonPropertyName("siblings")] public List<HierarchyEntity> Siblings { get; set; } = new();
}

/// <summary>Company data fingerprint.</summary>
public class Fingerprint
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("industrySector")] public string? IndustrySector { get; set; }
    [JsonPropertyName("industryGroup")] public string? IndustryGroup { get; set; }
    [JsonPropertyName("industry")] public string? Industry { get; set; }
    [JsonPropertyName("sizeCategory")] public string? SizeCategory { get; set; }
    [JsonPropertyName("employeeCountEstimate")] public int? EmployeeCountEstimate { get; set; }
    [JsonPropertyName("capitalAmount")] public double? CapitalAmount { get; set; }
    [JsonPropertyName("capitalCurrency")] public string? CapitalCurrency { get; set; }
    [JsonPropertyName("revenue")] public double? Revenue { get; set; }
    [JsonPropertyName("netIncome")] public double? NetIncome { get; set; }
    [JsonPropertyName("auditorTier")] public string? AuditorTier { get; set; }
    [JsonPropertyName("changeFrequency")] public long ChangeFrequency { get; set; }
    [JsonPropertyName("boardSize")] public long BoardSize { get; set; }
    [JsonPropertyName("companyAge")] public long CompanyAge { get; set; }
    [JsonPropertyName("canton")] public string Canton { get; set; } = "";
    [JsonPropertyName("legalForm")] public string LegalForm { get; set; } = "";
    [JsonPropertyName("hasParentCompany")] public bool HasParentCompany { get; set; }
    [JsonPropertyName("subsidiaryCount")] public long SubsidiaryCount { get; set; }
    [JsonPropertyName("generatedAt")] public string GeneratedAt { get; set; } = "";
    [JsonPropertyName("fingerprintVersion")] public string FingerprintVersion { get; set; } = "";
    /// <summary>Swiss register entry date (v3.1+).</summary>
    [JsonPropertyName("registrationDate")] public string? RegistrationDate { get; set; }
}

/// <summary>Query parameters for finding nearby companies.</summary>
public class NearbyParams
{
    public double Lat { get; set; }
    public double Lng { get; set; }
    public double? RadiusKm { get; set; }
    public long? Limit { get; set; }
}

/// <summary>A company near a geographic location.</summary>
public class NearbyCompany
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("distance")] public double Distance { get; set; }
    [JsonPropertyName("latitude")] public double Latitude { get; set; }
    [JsonPropertyName("longitude")] public double Longitude { get; set; }
}

/// <summary>Full company response including persons, changes, and relationships.</summary>
public class CompanyFullResponse
{
    [JsonPropertyName("company")] public Company Company { get; set; } = new();
    [JsonPropertyName("persons")] public List<PersonEntry> Persons { get; set; } = new();
    [JsonPropertyName("recentChanges")] public List<ChangeEntry> RecentChanges { get; set; } = new();
    [JsonPropertyName("relationships")] public List<RelationshipEntry> Relationships { get; set; } = new();
}

/// <summary>A person entry in a company full response.</summary>
public class PersonEntry
{
    [JsonPropertyName("personId")] public string? PersonId { get; set; }
    [JsonPropertyName("firstName")] public string? FirstName { get; set; }
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("since")] public string? Since { get; set; }
    [JsonPropertyName("until")] public string? Until { get; set; }
    // --- Enrichment provenance (v3.1+) ---
    [JsonPropertyName("roleSource")] public string? RoleSource { get; set; }
    [JsonPropertyName("roleConfidence")] public double? RoleConfidence { get; set; }
    [JsonPropertyName("roleInferredAt")] public string? RoleInferredAt { get; set; }
}

/// <summary>A change entry in a company full response.</summary>
public class ChangeEntry
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("changeType")] public string? ChangeType { get; set; }
    [JsonPropertyName("fieldName")] public string? FieldName { get; set; }
    [JsonPropertyName("oldValue")] public string? OldValue { get; set; }
    [JsonPropertyName("newValue")] public string? NewValue { get; set; }
    [JsonPropertyName("detectedAt")] public string DetectedAt { get; set; } = "";
    [JsonPropertyName("sourceDate")] public string? SourceDate { get; set; }
}

/// <summary>A relationship entry in a company full response.</summary>
public class RelationshipEntry
{
    [JsonPropertyName("relatedUid")] public string RelatedUid { get; set; } = "";
    [JsonPropertyName("relatedName")] public string? RelatedName { get; set; }
    [JsonPropertyName("relationshipType")] public string RelationshipType { get; set; } = "";
}

/// <summary>Industry classification for a company.</summary>
public class Classification
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("sectorCode")] public string? SectorCode { get; set; }
    [JsonPropertyName("sectorName")] public string? SectorName { get; set; }
    [JsonPropertyName("groupCode")] public string? GroupCode { get; set; }
    [JsonPropertyName("groupName")] public string? GroupName { get; set; }
    [JsonPropertyName("industryCode")] public string? IndustryCode { get; set; }
    [JsonPropertyName("industryName")] public string? IndustryName { get; set; }
    [JsonPropertyName("subIndustryCode")] public string? SubIndustryCode { get; set; }
    [JsonPropertyName("subIndustryName")] public string? SubIndustryName { get; set; }
    [JsonPropertyName("method")] public string Method { get; set; } = "";
    [JsonPropertyName("classifiedAt")] public string ClassifiedAt { get; set; } = "";
    [JsonPropertyName("auditorCategory")] public string? AuditorCategory { get; set; }
    [JsonPropertyName("isFinmaRegulated")] public bool IsFinmaRegulated { get; set; }
    // --- Enrichment provenance (v3.1+) ---
    [JsonPropertyName("industrySource")] public string? IndustrySource { get; set; }
    [JsonPropertyName("industryConfidence")] public double? IndustryConfidence { get; set; }
}

/// <summary>Corporate structure with head/branch offices and acquisitions.</summary>
public class CorporateStructure
{
    [JsonPropertyName("headOffices")] public List<RelatedCompanyEntry> HeadOffices { get; set; } = new();
    [JsonPropertyName("branchOffices")] public List<RelatedCompanyEntry> BranchOffices { get; set; } = new();
    [JsonPropertyName("acquisitions")] public List<RelatedCompanyEntry> Acquisitions { get; set; } = new();
    [JsonPropertyName("acquiredBy")] public List<RelatedCompanyEntry> AcquiredBy { get; set; } = new();
}

/// <summary>A related company in a corporate structure.</summary>
public class RelatedCompanyEntry
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
}

/// <summary>An acquisition record.</summary>
public class Acquisition
{
    [JsonPropertyName("acquirerUid")] public string AcquirerUid { get; set; } = "";
    [JsonPropertyName("acquiredUid")] public string AcquiredUid { get; set; } = "";
    [JsonPropertyName("acquirerName")] public string? AcquirerName { get; set; }
    [JsonPropertyName("acquiredName")] public string? AcquiredName { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}

/// <summary>A company note.</summary>
public class Note
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("content")] public string Content { get; set; } = "";
    [JsonPropertyName("noteType")] public string NoteType { get; set; } = "";
    [JsonPropertyName("rating")] public int? Rating { get; set; }
    [JsonPropertyName("isPrivate")] public bool IsPrivate { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
    [JsonPropertyName("updatedAt")] public string UpdatedAt { get; set; } = "";
}

/// <summary>Request body for creating a note.</summary>
public class CreateNoteRequest
{
    [JsonPropertyName("content")] public string Content { get; set; } = "";
    [JsonPropertyName("noteType")] public string? NoteType { get; set; }
    [JsonPropertyName("rating")] public int? Rating { get; set; }
    [JsonPropertyName("isPrivate")] public bool? IsPrivate { get; set; }
}

/// <summary>Request body for updating a note.</summary>
public class UpdateNoteRequest
{
    [JsonPropertyName("content")] public string? Content { get; set; }
    [JsonPropertyName("noteType")] public string? NoteType { get; set; }
    [JsonPropertyName("rating")] public int? Rating { get; set; }
    [JsonPropertyName("isPrivate")] public bool? IsPrivate { get; set; }
}

/// <summary>A company tag.</summary>
public class Tag
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("tagName")] public string TagName { get; set; } = "";
    [JsonPropertyName("color")] public string? Color { get; set; }
    [JsonPropertyName("createdAt")] public string CreatedAt { get; set; } = "";
}

/// <summary>Request body for creating a tag.</summary>
public class CreateTagRequest
{
    [JsonPropertyName("tagName")] public string TagName { get; set; } = "";
    [JsonPropertyName("color")] public string? Color { get; set; }
}

/// <summary>Tag summary with usage count.</summary>
public class TagSummary
{
    [JsonPropertyName("tagName")] public string TagName { get; set; } = "";
    [JsonPropertyName("count")] public long Count { get; set; }
}

/// <summary>Request body for Excel export.</summary>
public class ExcelExportRequest
{
    [JsonPropertyName("uids")] public List<string>? Uids { get; set; }
    [JsonPropertyName("filter")] public ExcelExportFilter? Filter { get; set; }
    [JsonPropertyName("fields")] public List<string>? Fields { get; set; }
}

/// <summary>Filter for Excel export.</summary>
public class ExcelExportFilter
{
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("search")] public string? Search { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("auditorCategory")] public string? AuditorCategory { get; set; }
}
