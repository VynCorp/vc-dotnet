# VynCo .NET SDK

[![NuGet](https://img.shields.io/nuget/v/VynCo)](https://www.nuget.org/packages/VynCo)
[![License](https://img.shields.io/github/license/VynCorp/vc-dotnet)](LICENSE)

.NET SDK for the [VynCo](https://vynco.ch) Swiss Corporate Intelligence API. Access 320,000+ Swiss companies from the Zefix commercial register with typed clients, automatic retries, credit tracking headers, and structured error handling.

## Installation

```
dotnet add package VynCo
```

**Targets:** .NET Standard 2.0 (.NET Framework 4.6.1+, .NET Core 2.0+) and .NET 10.0.

## Quick start

```csharp
using VynCo;
using VynCo.Models;

using var client = new VynCoClient("vc_live_your_api_key");

// Search companies
var results = await client.Companies.SearchAsync(
    new CompanySearchRequest { Query = "Novartis", Limit = 5 });

// Get company by Swiss UID
var company = await client.Companies.GetAsync("CHE-123.456.789");
Console.WriteLine($"{company.Name} ({company.Canton}) - {company.Status}");

// Check credit usage from response headers
Console.WriteLine($"Credits used: {client.LastResponseHeaders?.CreditsUsed}");
Console.WriteLine($"Remaining: {client.LastResponseHeaders?.CreditsRemaining}");

// List recent registry changes
var changes = await client.Changes.ListAsync(
    new ListChangesParams { Page = 1, PageSize = 10 });

// Check credit balance
var balance = await client.Credits.BalanceAsync();
Console.WriteLine($"Credits remaining: {balance.Balance}/{balance.MonthlyCredits}");
```

## Resources

| Resource | Methods | Description |
|----------|---------|-------------|
| `client.Companies` | `ListAsync`, `GetAsync`, `CountAsync`, `StatisticsAsync`, `SearchAsync`, `BatchAsync`, `CompareAsync`, `GetRelationshipsAsync`, `GetHierarchyAsync`, `GetNewsAsync`, `GetReportsAsync` | Swiss company data, relationships, news, reports |
| `client.Changes` | `ListAsync`, `GetByCompanyAsync`, `StatisticsAsync`, `GetBySogcIdAsync`, `ReviewAsync`, `BatchAsync` | Registry change tracking, SOGC lookup, review |
| `client.Persons` | `ListAsync`, `GetAsync`, `GetRolesAsync`, `GetConnectionsAsync`, `GetBoardMembersAsync`, `NetworkStatsAsync` | Board members, executives, and networks |
| `client.Dossiers` | `GetAsync`, `ListAsync`, `GenerateAsync`, `StatisticsAsync` | AI-powered company intelligence dossiers |
| `client.Analytics` | `ClusterAsync`, `DetectAnomaliesAsync`, `CohortsAsync`, `CantonsAsync`, `AuditorsAsync`, `RfmSegmentsAsync`, `VelocityAsync` | Clustering, anomalies, cohorts, segmentation |
| `client.Watches` | `ListAsync`, `AddAsync`, `RemoveAsync`, `ListNotificationsAsync` | Company watch subscriptions and notifications |
| `client.News` | `GetRecentAsync` | Recent news across all companies |
| `client.ApiKeys` | `CreateAsync`, `ListAsync`, `RevokeAsync` | API key lifecycle management |
| `client.Credits` | `BalanceAsync`, `UsageAsync`, `HistoryAsync` | Credit balance, usage, and transaction ledger |
| `client.Billing` | `CreateCheckoutSessionAsync`, `CreatePortalSessionAsync` | Stripe billing integration |
| `client.Teams` | `CreateAsync`, `GetCurrentAsync`, `ListMembersAsync`, `InviteMemberAsync`, `UpdateMemberRoleAsync`, `RemoveMemberAsync`, `BillingSummaryAsync` | Team and member management |
| `client.SyncStatus` | `GetAsync` | Zefix registry sync status |
| `client.Health` | `CheckAsync` | API health check |

## Configuration

```csharp
using var client = new VynCoClient(
    apiKey: "vc_live_your_api_key",
    baseUrl: "https://api.vynco.ch",  // default
    maxRetries: 2,                     // default; retries on 429 and 5xx
    timeout: TimeSpan.FromSeconds(30)  // default
);
```

The client authenticates with a Bearer token. Use `vc_live_*` keys for production and `vc_test_*` keys for testing (test keys do not consume credits).

## Response headers

Every API response includes credit and rate-limit metadata, accessible via `client.LastResponseHeaders`:

```csharp
await client.Companies.GetAsync("CHE-123.456.789");

var headers = client.LastResponseHeaders;
Console.WriteLine($"Request ID:   {headers?.RequestId}");
Console.WriteLine($"Credits used: {headers?.CreditsUsed}");
Console.WriteLine($"Remaining:    {headers?.CreditsRemaining}");
Console.WriteLine($"Rate limit:   {headers?.RateLimitLimit} req/min");
Console.WriteLine($"Data source:  {headers?.DataSource}");
```

## Error handling

All API errors are mapped to typed exceptions with RFC 7807 Problem Details:

```csharp
try
{
    var company = await client.Companies.GetAsync("CHE-000.000.000");
}
catch (NotFoundException ex)
{
    Console.WriteLine($"Not found: {ex.Message}");
}
catch (RateLimitException)
{
    // Automatic retry handles transient 429s;
    // this fires only after maxRetries is exhausted
}
catch (InsufficientCreditsException)
{
    // Upgrade plan or wait for monthly credit reset
}
catch (VynCoException ex)
{
    // Catch-all for any API error
    Console.WriteLine($"HTTP {ex.StatusCode}: {ex.Body?.Detail}");
}
```

| Exception | Status | When |
|-----------|--------|------|
| `BadRequestException` | 400 | Invalid request parameters |
| `AuthenticationException` | 401 | Invalid or missing API key |
| `InsufficientCreditsException` | 402 | Credit balance exhausted |
| `ForbiddenException` | 403 | Insufficient permissions |
| `NotFoundException` | 404 | Entity not found |
| `ConflictException` | 409 | Conflicting state (e.g., duplicate invitation) |
| `ValidationException` | 422 | Request validation failed |
| `RateLimitException` | 429 | Rate limit exceeded (after retries) |
| `ServerException` | 5xx | Server error (after retries) |

## Pagination

```csharp
var page = await client.Companies.ListAsync(
    new ListCompaniesParams { Page = 1, PageSize = 50, Canton = "ZH" });

Console.WriteLine($"Showing {page.Items.Count} of {page.TotalCount} ({page.TotalPages} pages)");

while (page.HasNextPage)
{
    page = await client.Companies.ListAsync(
        new ListCompaniesParams { Page = page.Page + 1, PageSize = 50, Canton = "ZH" });
    // process page.Items
}
```

## Batch operations

Look up multiple companies in a single request (up to 50 UIDs):

```csharp
var companies = await client.Companies.BatchAsync(
    new BatchLookupRequest { Uids = new List<string> { "CHE-123.456.789", "CHE-987.654.321" } });
```

## Company watches

Subscribe to change notifications for companies you're monitoring:

```csharp
// Add a watch
var watch = await client.Watches.AddAsync(new AddWatchRequest
{
    CompanyUid = "CHE-123.456.789",
    Channel = "InApp",
    WatchedChangeTypes = new List<string> { "NameChange", "AuditorChange" }
});

// List notifications
var notifications = await client.Watches.ListNotificationsAsync(limit: 20);
```

## Analytics

Run advanced analytics on the Swiss corporate registry:

```csharp
// K-Means clustering
var clusters = await client.Analytics.ClusterAsync(
    new ClusteringRequest { K = 5, Canton = "ZH", Limit = 1000 });

// Change velocity over the last 7 days
var velocity = await client.Analytics.VelocityAsync(
    new VelocityParams { Days = 7 });
```

## License

[Apache-2.0](LICENSE)
