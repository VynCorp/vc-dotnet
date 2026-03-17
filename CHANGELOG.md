# Changelog

All notable changes to the VynCo .NET SDK will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2026-03-17

### Added

- `VynCoClient` main entry point with configurable API key, base URL, retry count, and timeout
- Bearer token authentication (`vc_live_*` / `vc_test_*` API keys)
- Automatic retry with exponential backoff on 429 (rate limit) and 5xx (server error) responses
- RFC 7807 Problem Details error mapping with typed exception hierarchy:
  - `AuthenticationException` (401)
  - `InsufficientCreditsException` (402)
  - `NotFoundException` (404)
  - `ConflictException` (409)
  - `ValidationException` (422)
  - `RateLimitException` (429)
  - `ServerException` (5xx)
- **Companies resource** — `ListAsync`, `GetAsync`, `CountAsync`, `StatisticsAsync`, `SearchAsync`, `BatchAsync`
- **Changes resource** — `ListAsync`, `GetByCompanyAsync`, `StatisticsAsync`, `ReviewAsync`, `BatchAsync`
- **Persons resource** — `SearchAsync`, `GetAsync`, `GetRolesAsync`, `GetBoardMembersAsync`
- **Dossiers resource** — `GetAsync`, `ListAsync`, `GenerateAsync`, `StatisticsAsync`
- **API Keys resource** — `CreateAsync`, `ListAsync`, `RevokeAsync`
- **Credits resource** — `BalanceAsync`, `UsageAsync`, `HistoryAsync`
- **Billing resource** — `CreateCheckoutSessionAsync`, `CreatePortalSessionAsync`
- **Teams resource** — `CreateAsync`, `GetCurrentAsync`
- **Webhooks resource** — `CreateAsync`, `ListAsync`, `GetAsync`, `DeleteAsync`, `TestAsync`
- **Sync Status resource** — `GetAsync`
- Generic `PagedResponse<T>` for paginated endpoints
- `BatchLookupRequest` for batch operations (up to 500 UIDs)
- Multi-targeting: `netstandard2.0` and `net10.0`

[0.1.0]: https://github.com/VynCorp/vc-dotnet/releases/tag/v0.1.0
