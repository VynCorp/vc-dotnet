# VynCo .NET SDK

[![NuGet](https://img.shields.io/nuget/v/VynCo)](https://www.nuget.org/packages/VynCo)
[![License](https://img.shields.io/github/license/VynCorp/vc-dotnet)](LICENSE)

.NET SDK for the [VynCo](https://vynco.ch) Swiss Corporate Intelligence API. Access 320,000+ Swiss companies from the Zefix commercial register with typed clients, automatic retries, and structured error handling.

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
| `client.Companies` | `ListAsync`, `GetAsync`, `CountAsync`, `StatisticsAsync`, `SearchAsync`, `BatchAsync` | Swiss company data from the Zefix register |
| `client.Changes` | `ListAsync`, `GetByCompanyAsync`, `StatisticsAsync`, `ReviewAsync`, `BatchAsync` | Registry change tracking and review |
| `client.Persons` | `SearchAsync`, `GetAsync`, `GetRolesAsync`, `GetBoardMembersAsync` | Board members and executives |
| `client.Dossiers` | `GetAsync`, `ListAsync`, `GenerateAsync`, `StatisticsAsync` | AI-powered company intelligence dossiers |
| `client.ApiKeys` | `CreateAsync`, `ListAsync`, `RevokeAsync` | API key lifecycle management |
| `client.Credits` | `BalanceAsync`, `UsageAsync`, `HistoryAsync` | Credit balance, usage, and transaction ledger |
| `client.Billing` | `CreateCheckoutSessionAsync`, `CreatePortalSessionAsync` | Stripe billing integration |
| `client.Teams` | `CreateAsync`, `GetCurrentAsync` | Team management |
| `client.Webhooks` | `CreateAsync`, `ListAsync`, `GetAsync`, `DeleteAsync`, `TestAsync` | Webhook CRUD and testing |
| `client.SyncStatus` | `GetAsync` | Zefix registry sync status |

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
| `AuthenticationException` | 401 | Invalid or missing API key |
| `InsufficientCreditsException` | 402 | Credit balance exhausted |
| `NotFoundException` | 404 | Entity not found |
| `ConflictException` | 409 | Conflicting state |
| `ValidationException` | 422 | Invalid request parameters |
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

Look up multiple companies in a single request (up to 500 UIDs):

```csharp
var companies = await client.Companies.BatchAsync(
    new BatchLookupRequest { Uids = new List<string> { "CHE-123.456.789", "CHE-987.654.321" } });
```

## License

[Apache-2.0](LICENSE)
