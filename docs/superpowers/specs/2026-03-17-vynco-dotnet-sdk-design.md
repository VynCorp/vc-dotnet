# VynCo .NET SDK Design

## Overview

A .NET SDK for the VynCo Swiss Corporate Intelligence API (`https://api.vynco.ch`), following the same resource-based architecture as the VynFi .NET SDK.

## Architecture

- **VynCoClient** — Main entry point. Constructor: `(string apiKey, string baseUrl, int maxRetries, TimeSpan? timeout)`. Implements `IDisposable`. Owns `HttpClient`. Exposes 10 resource properties.
- **Resources** — One class per API domain. Internal constructor taking `VynCoClient`. Async methods with `CancellationToken`.
- **Models** — Request/response DTOs with `[JsonPropertyName("camelCase")]`. Defaults for strings, nullable for optional fields, `DateTime?` for dates.
- **Exceptions** — `VynCoException` base (maps RFC 7807 ProblemDetails). Subtypes for 401, 402, 404, 409, 422, 429, 5xx.
- **HTTP** — Retry on 429/5xx with exponential backoff. Bearer token auth. `ConfigureAwait(false)` throughout.

## Package

- **PackageId:** VynCo
- **Namespace:** VynCo
- **Targets:** netstandard2.0 + net10.0
- **Version:** 0.1.0
- **License:** Apache-2.0
- **No DI extensions** — users wire up manually.

## Resource Coverage

### Companies
| Method | Endpoint | Returns |
|--------|----------|---------|
| ListAsync | GET /v1/companies | PagedResponse\<Company\> |
| GetAsync | GET /v1/companies/{uid} | Company |
| CountAsync | GET /v1/companies/count | CompanyCount |
| StatisticsAsync | GET /v1/companies/statistics | CompanyStatistics |
| SearchAsync | POST /v1/companies/search | List\<Company\> |
| BatchAsync | POST /v1/companies/batch | List\<Company\> |

### Changes
| Method | Endpoint | Returns |
|--------|----------|---------|
| ListAsync | GET /v1/changes | PagedResponse\<Change\> |
| GetByCompanyAsync | GET /v1/changes/{uid} | List\<Change\> |
| StatisticsAsync | GET /v1/changes/statistics | ChangeStatistics |
| ReviewAsync | PUT /v1/changes/{id}/review | ReviewResult |
| BatchAsync | POST /v1/changes/batch | List\<Change\> |

### Persons
| Method | Endpoint | Returns |
|--------|----------|---------|
| SearchAsync | GET /v1/persons | List\<Person\> |
| GetAsync | GET /v1/persons/{id} | Person |
| GetRolesAsync | GET /v1/persons/{id}/roles | List\<PersonConnection\> |
| GetBoardMembersAsync | GET /v1/persons/board-members/{uid} | List\<BoardMember\> |

### Dossiers
| Method | Endpoint | Returns |
|--------|----------|---------|
| GetAsync | GET /v1/dossiers/{uid} | Dossier |
| ListAsync | GET /v1/dossiers | List\<Dossier\> |
| GenerateAsync | POST /v1/dossiers/{uid}/generate | DossierGenerated |
| StatisticsAsync | GET /v1/dossiers/statistics | DossierStatistics |

### ApiKeys
| Method | Endpoint | Returns |
|--------|----------|---------|
| CreateAsync | POST /v1/api-keys | ApiKeyCreated |
| ListAsync | GET /v1/api-keys | List\<ApiKey\> |
| RevokeAsync | DELETE /v1/api-keys/{id} | void |

### Credits
| Method | Endpoint | Returns |
|--------|----------|---------|
| BalanceAsync | GET /v1/credits/balance | CreditBalance |
| UsageAsync | GET /v1/credits/usage | UsageBreakdown |
| HistoryAsync | GET /v1/credits/history | List\<CreditLedgerEntry\> |

### Billing
| Method | Endpoint | Returns |
|--------|----------|---------|
| CreateCheckoutSessionAsync | POST /v1/billing/checkout-session | SessionUrl |
| CreatePortalSessionAsync | POST /v1/billing/portal-session | SessionUrl |

### Teams
| Method | Endpoint | Returns |
|--------|----------|---------|
| CreateAsync | POST /v1/teams | Team |
| GetCurrentAsync | GET /v1/teams/me | Team |

### Webhooks
| Method | Endpoint | Returns |
|--------|----------|---------|
| CreateAsync | POST /v1/webhooks | WebhookCreated |
| ListAsync | GET /v1/webhooks | List\<Webhook\> |
| GetAsync | GET /v1/webhooks/{id} | Webhook |
| DeleteAsync | DELETE /v1/webhooks/{id} | void |
| TestAsync | POST /v1/webhooks/{id}/test | JsonElement |

### SyncStatus
| Method | Endpoint | Returns |
|--------|----------|---------|
| GetAsync | GET /v1/sync/status | List\<SyncStatus\> |

## Error Handling

Maps RFC 7807 ProblemDetails responses. Exception hierarchy:
- `VynCoException` (base) — `StatusCode`, `Body` (ProblemDetails)
- `AuthenticationException` (401)
- `InsufficientCreditsException` (402)
- `NotFoundException` (404)
- `ConflictException` (409)
- `ValidationException` (422)
- `RateLimitException` (429)
- `ServerException` (5xx)

## Pagination

Generic `PagedResponse<T>` with `Items`, `TotalCount`, `Page`, `PageSize`, `TotalPages`, `HasPreviousPage`, `HasNextPage`.

## JSON Serialization

- camelCase property names (matching the backend)
- Explicit `[JsonPropertyName]` on all DTO properties
- `PropertyNamingPolicy = null` in serializer options
- `DefaultIgnoreCondition = WhenWritingNull`
- `PropertyNameCaseInsensitive = true`

## Testing

- xUnit with `MockHttpHandler` and `TestHelper`
- Reflection-based HttpClient injection (same as VynFi)
- Target: net10.0
- Coverage: auth headers, exception mapping, request construction, response deserialization, pagination

## File Structure

```
vc-dotnet/
├── VynCo.slnx
├── src/VynCo/
│   ├── VynCo.csproj
│   ├── VynCoClient.cs
│   ├── Exceptions.cs
│   ├── Models/
│   │   ├── Companies.cs, Changes.cs, Persons.cs, Dossiers.cs
│   │   ├── ApiKeys.cs, Credits.cs, Billing.cs
│   │   ├── Teams.cs, Webhooks.cs, SyncStatus.cs
│   │   └── Common.cs (PagedResponse<T>, BatchLookupRequest)
│   └── Resources/
│       ├── CompaniesResource.cs, ChangesResource.cs
│       ├── PersonsResource.cs, DossiersResource.cs
│       ├── ApiKeysResource.cs, CreditsResource.cs
│       ├── BillingResource.cs, TeamsResource.cs
│       ├── WebhooksResource.cs, SyncStatusResource.cs
└── tests/VynCo.Tests/
    ├── VynCo.Tests.csproj
    ├── MockHttpHandler.cs
    ├── TestHelper.cs
    └── VynCoClientTests.cs
```
