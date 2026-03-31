# VynCo .NET SDK

[![NuGet](https://img.shields.io/nuget/v/VynCo)](https://www.nuget.org/packages/VynCo)
[![CI](https://github.com/VynCorp/vc-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/VynCorp/vc-dotnet/actions/workflows/ci.yml)
[![License](https://img.shields.io/github/license/VynCorp/vc-dotnet)](LICENSE)

.NET SDK for the [VynCo](https://vynco.ch) Swiss Corporate Intelligence API. Access 500,000+ Swiss companies from the commercial register with change tracking, sanctions screening, AI-powered risk analysis, network graphs, watchlists, webhooks, and bulk data exports.

## Installation

```
dotnet add package VynCo
```

**Targets:** .NET Standard 2.0 (.NET Framework 4.6.1+, .NET Core 2.0+) and .NET 10.0.

## Quick Start

```csharp
using VynCo;
using VynCo.Models;

using var client = new VynCoClient("vc_live_your_api_key");

// List companies with filtering
var result = await client.Companies.ListAsync(
    new CompanyListParams { Search = "Novartis", Canton = "BS" });
Console.WriteLine($"Found {result.Total} companies");

// Get a single company
var company = await client.Companies.GetAsync("CHE-105.805.080");
Console.WriteLine($"{company.Name}: {company.LegalForm}");

// Sanctions screening
var screening = await client.Screening.ScreenAsync(
    new ScreeningRequest { Name = "Suspicious Corp" });
Console.WriteLine($"Risk: {screening.RiskLevel} ({screening.HitCount} hits)");

// AI risk score
var risk = await client.Ai.RiskScoreAsync(
    new RiskScoreRequest { Uid = "CHE-105.805.080" });
Console.WriteLine($"Risk score: {risk.OverallScore}/100 ({risk.RiskLevel})");

// Credit balance
var credits = await client.Credits.BalanceAsync();
Console.WriteLine($"Credits remaining: {credits.Balance}");
```

## API Coverage

18 resource modules covering 69 endpoints:

| Resource | Methods |
|----------|---------|
| `client.Health` | `CheckAsync` |
| `client.Companies` | `ListAsync`, `GetAsync`, `CountAsync`, `EventsAsync`, `StatisticsAsync`, `CompareAsync`, `NewsAsync`, `ReportsAsync`, `RelationshipsAsync`, `HierarchyAsync`, `FingerprintAsync`, `NearbyAsync` |
| `client.Auditors` | `HistoryAsync`, `TenuresAsync` |
| `client.Dashboard` | `GetAsync` |
| `client.Screening` | `ScreenAsync` |
| `client.Watchlists` | `ListAsync`, `CreateAsync`, `DeleteAsync`, `CompaniesAsync`, `AddCompaniesAsync`, `RemoveCompanyAsync`, `EventsAsync` |
| `client.Webhooks` | `ListAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `TestAsync`, `DeliveriesAsync` |
| `client.Exports` | `CreateAsync`, `GetAsync`, `DownloadAsync` |
| `client.Ai` | `DossierAsync`, `SearchAsync`, `RiskScoreAsync` |
| `client.ApiKeys` | `ListAsync`, `CreateAsync`, `RevokeAsync` |
| `client.Credits` | `BalanceAsync`, `UsageAsync`, `HistoryAsync` |
| `client.Billing` | `CreateCheckoutAsync`, `CreatePortalAsync` |
| `client.Teams` | `MeAsync`, `CreateAsync`, `MembersAsync`, `InviteMemberAsync`, `UpdateMemberRoleAsync`, `RemoveMemberAsync`, `BillingSummaryAsync` |
| `client.Changes` | `ListAsync`, `ByCompanyAsync`, `StatisticsAsync` |
| `client.Persons` | `BoardMembersAsync` |
| `client.Analytics` | `CantonsAsync`, `AuditorsAsync`, `ClusterAsync`, `AnomaliesAsync`, `RfmSegmentsAsync`, `CohortsAsync`, `CandidatesAsync` |
| `client.Dossiers` | `CreateAsync`, `ListAsync`, `GetAsync`, `DeleteAsync` |
| `client.Graph` | `GetAsync`, `ExportAsync`, `AnalyzeAsync` |

## Response Metadata

Every API response includes header metadata for credit tracking and rate limiting:

```csharp
await client.Companies.GetAsync("CHE-105.805.080");

var h = client.LastResponseHeaders;
Console.WriteLine($"Request ID:          {h?.RequestId}");          // X-Request-Id
Console.WriteLine($"Credits used:        {h?.CreditsUsed}");        // X-Credits-Used
Console.WriteLine($"Credits remaining:   {h?.CreditsRemaining}");   // X-Credits-Remaining
Console.WriteLine($"Rate limit:          {h?.RateLimitLimit}");     // X-RateLimit-Limit
Console.WriteLine($"Rate limit remaining:{h?.RateLimitRemaining}"); // X-RateLimit-Remaining
Console.WriteLine($"Rate limit reset:    {h?.RateLimitReset}");     // X-RateLimit-Reset
Console.WriteLine($"Data source:         {h?.DataSource}");         // X-Data-Source
```

## Configuration

```csharp
using var client = new VynCoClient(
    apiKey: "vc_live_your_api_key",
    baseUrl: "https://api.vynco.ch",  // default
    maxRetries: 2,                     // default; retries on 429 and 5xx
    timeout: TimeSpan.FromSeconds(30)  // default
);
```

The client automatically retries on HTTP 429 (rate limited) and 5xx (server error) with exponential backoff (500ms x 2^attempt). It respects the `Retry-After` header when present.

## Error Handling

All API errors are mapped to typed exceptions with RFC 7807 Problem Details:

```csharp
try
{
    var company = await client.Companies.GetAsync("CHE-000.000.000");
}
catch (AuthenticationException) { Console.WriteLine("Invalid API key"); }
catch (InsufficientCreditsException) { Console.WriteLine("Top up credits"); }
catch (ForbiddenException) { Console.WriteLine("Insufficient permissions"); }
catch (NotFoundException ex) { Console.WriteLine($"Not found: {ex.Body?.Detail}"); }
catch (ValidationException ex) { Console.WriteLine($"Bad request: {ex.Body?.Detail}"); }
catch (ConflictException) { Console.WriteLine("Resource conflict"); }
catch (RateLimitException) { Console.WriteLine("Rate limited, retry later"); }
catch (ServerException) { Console.WriteLine("Server error"); }
catch (VynCoException ex) { Console.WriteLine($"Error: {ex.Message}"); }
```

Error bodies follow [RFC 7807 Problem Details](https://tools.ietf.org/html/rfc7807) with `Type`, `Title`, `Status`, `Detail`, and `Instance` fields.

## Pagination

```csharp
var page = await client.Companies.ListAsync(
    new CompanyListParams { Canton = "ZH", Page = 1, PageSize = 50 });

foreach (var company in page.Items)
    Console.WriteLine($"{company.Uid}: {company.Name}");
```

## Watchlists

```csharp
// Create a watchlist
var watchlist = await client.Watchlists.CreateAsync(
    new CreateWatchlistRequest { Name = "Portfolio", Description = "Tracked companies" });

// Add companies
await client.Watchlists.AddCompaniesAsync(watchlist.Id,
    new AddCompaniesRequest { Uids = new List<string> { "CHE-105.805.080", "CHE-109.340.740" } });

// Get events
var events = await client.Watchlists.EventsAsync(watchlist.Id, limit: 20);
```

## Analytics

```csharp
// Clustering analysis
var clusters = await client.Analytics.ClusterAsync(
    new ClusterRequest { Algorithm = "kmeans", K = 5 });

// Canton distribution
var cantons = await client.Analytics.CantonsAsync();

// Audit candidates
var candidates = await client.Analytics.CandidatesAsync(
    new CandidateParams { Canton = "ZH", SortBy = "shareCapital" });
```

## License

[Apache-2.0](LICENSE)
