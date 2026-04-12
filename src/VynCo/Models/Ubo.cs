using System.Text.Json.Serialization;

namespace VynCo.Models;

// Non-Swiss parent entities resolved via GLEIF appear with synthetic
// identifiers of the form `LEI:{20-char-lei}` in the *Uid fields of
// UboPerson, ChainLink, and OwnershipLink. These are NOT resolvable via
// companies.get().

/// <summary>A natural person identified as an ultimate beneficial owner (v3.1+).</summary>
public class UboPerson
{
    [JsonPropertyName("personId")] public long PersonId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("controllingEntityUid")] public string ControllingEntityUid { get; set; } = "";
    [JsonPropertyName("controllingEntityName")] public string ControllingEntityName { get; set; } = "";
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("signingAuthority")] public string? SigningAuthority { get; set; }
    [JsonPropertyName("pathLength")] public int PathLength { get; set; }
}

/// <summary>A single link in an ownership chain (v3.1+).</summary>
public class ChainLink
{
    [JsonPropertyName("fromUid")] public string FromUid { get; set; } = "";
    [JsonPropertyName("fromName")] public string FromName { get; set; } = "";
    [JsonPropertyName("toUid")] public string ToUid { get; set; } = "";
    [JsonPropertyName("toName")] public string ToName { get; set; } = "";
    [JsonPropertyName("depth")] public int Depth { get; set; }
}

/// <summary>Ultimate beneficial owner resolution response (v3.1+).</summary>
public class UboResponse
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("uboPersons")] public List<UboPerson> UboPersons { get; set; } = new();
    [JsonPropertyName("ownershipChain")] public List<ChainLink> OwnershipChain { get; set; } = new();
    [JsonPropertyName("chainDepth")] public int ChainDepth { get; set; }
    [JsonPropertyName("riskFlags")] public List<string> RiskFlags { get; set; } = new();
    /// <summary>Human-readable explanation when the chain can't be fully resolved.</summary>
    [JsonPropertyName("dataCoverageNote")] public string? DataCoverageNote { get; set; }
}

/// <summary>Request body for ownership chain trace (v3.1+).</summary>
public class OwnershipRequest
{
    [JsonPropertyName("maxDepth")] public int? MaxDepth { get; set; }
}

/// <summary>A company entity in an ownership chain (v3.1+).</summary>
public class OwnershipEntity
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("canton")] public string? Canton { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("legalForm")] public string? LegalForm { get; set; }
    [JsonPropertyName("shareCapital")] public double? ShareCapital { get; set; }
}

/// <summary>A single directional relationship in an ownership chain (v3.1+).</summary>
public class OwnershipLink
{
    [JsonPropertyName("sourceUid")] public string SourceUid { get; set; } = "";
    [JsonPropertyName("sourceName")] public string SourceName { get; set; } = "";
    [JsonPropertyName("targetUid")] public string TargetUid { get; set; } = "";
    [JsonPropertyName("targetName")] public string TargetName { get; set; } = "";
    /// <summary><c>head_office</c> | <c>branch_office</c> | <c>acquisition</c> | <c>gleif_parent</c>.</summary>
    [JsonPropertyName("relationshipType")] public string RelationshipType { get; set; } = "";
    [JsonPropertyName("depth")] public int Depth { get; set; }
}

/// <summary>A person's role at a specific company in an ownership chain (v3.1+).</summary>
public class PersonCompanyRole
{
    [JsonPropertyName("companyUid")] public string CompanyUid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("role")] public string Role { get; set; } = "";
}

/// <summary>A person with significant roles across the ownership chain (v3.1+).</summary>
public class KeyPerson
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("companies")] public List<PersonCompanyRole> Companies { get; set; } = new();
}

/// <summary>A detected circular ownership pattern (v3.1+).</summary>
public class CircularFlag
{
    [JsonPropertyName("loopUids")] public List<string> LoopUids { get; set; } = new();
    [JsonPropertyName("description")] public string Description { get; set; } = "";
}

/// <summary>Full ownership trace response from <c>POST /ownership/{uid}</c> (v3.1+).</summary>
public class OwnershipResponse
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "";
    [JsonPropertyName("companyName")] public string CompanyName { get; set; } = "";
    [JsonPropertyName("ownershipChain")] public List<OwnershipLink> OwnershipChain { get; set; } = new();
    [JsonPropertyName("ultimateParent")] public OwnershipEntity? UltimateParent { get; set; }
    [JsonPropertyName("keyPersons")] public List<KeyPerson> KeyPersons { get; set; } = new();
    [JsonPropertyName("circularFlags")] public List<CircularFlag> CircularFlags { get; set; } = new();
    [JsonPropertyName("riskLevel")] public string RiskLevel { get; set; } = "";
    [JsonPropertyName("assessedAt")] public string AssessedAt { get; set; } = "";
}
