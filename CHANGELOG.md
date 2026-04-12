# Changelog

All notable changes to the VynCo .NET SDK will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.1.0] - 2026-04-12

Alignment release matching the reference Python SDK (`vynco` v3.1.0), Rust SDK
(`vynco` v2.3.0), and TypeScript SDK (`@vynco/sdk` v3.1.0). Adds the full v3.1
API surface: historical timelines, UBO resolution, similar companies,
media-with-sentiment, person-centric networks, market flow analytics, canton
migrations, industry benchmarking, batch operations, and saved alerts.

### Added

**2 new resources:**

- **`client.Alerts`** — saved queries with optional webhook delivery (`ListAsync`, `CreateAsync`, `DeleteAsync`)
- **`client.Ownership`** — ownership-chain trace with circular-ownership detection (`TraceAsync`)

**14 new methods on existing resources:**

- `Companies.TimelineAsync(uid, params?)` — chronological event timeline
- `Companies.TimelineSummaryAsync(uid, params?)` — AI-generated narrative summary
- `Companies.SimilarAsync(uid, params?)` — similar-company scoring on industry/canton/capital/legal form/auditor tier
- `Companies.UboAsync(uid)` — ultimate beneficial owner resolution
- `Companies.MediaAsync(uid, params?)` — news items with optional sentiment filter
- `Companies.MediaAnalyzeAsync(uid)` — trigger LLM sentiment analysis
- `Companies.ExportCsvAsync(request)` — canonical CSV export (replaces `ExportExcelAsync`, kept as `[Obsolete]` alias)
- `Persons.NetworkAsync(id)` — person-centric network (companies + co-directors + stats)
- `Persons.BoardMembersPagedAsync(uid, params?)` — new paginated variant (unpaginated `BoardMembersAsync(uid)` unchanged)
- `Analytics.FlowsAsync(params?)` — registration/dissolution flows over time
- `Analytics.MigrationsAsync(params?)` — canton-to-canton legal-seat migrations
- `Analytics.BenchmarkAsync(uid, params?)` — company vs industry-peer percentile ranks
- `Screening.BatchAsync(request)` — batch sanctions screening (up to 100 UIDs)
- `Ai.RiskScoreBatchAsync(request)` — batch AI risk scoring (up to 50 UIDs)

**New model classes (40+):**

- `HierarchyEntity` (replaces `JsonElement` on `HierarchyResponse.Parent/Subsidiaries/Siblings`)
- Timeline: `TimelineParams`, `TimelineEvent`, `TimelineResponse`, `TimelineSummaryResponse`
- Similar: `SimilarParams`, `SimilarCompanyResult`, `SimilarCompaniesResponse`
- UBO/Ownership: `UboPerson`, `ChainLink`, `UboResponse`, `OwnershipRequest`, `OwnershipEntity`, `OwnershipLink`, `PersonCompanyRole`, `KeyPerson`, `CircularFlag`, `OwnershipResponse`
- Media: `MediaParams`, `MediaItem`, `MediaResponse`, `MediaAnalysisResponse`
- Alerts: `Alert`, `CreateAlertRequest`
- Flows/Migrations/Benchmark: `FlowsParams`, `FlowDataPoint`, `FlowsResponse`, `MigrationsParams`, `MigrationFlow`, `MigrationResponse`, `BenchmarkParams`, `BenchmarkDimension`, `BenchmarkResponse`
- Batch: `BatchScreeningRequest`, `BatchScreeningHitSummary`, `BatchScreeningResultByUid`, `BatchScreeningResponse`, `BatchRiskScoreRequest`, `RiskScoreResult`, `BatchRiskScoreResponse`
- Person network: `NetworkPerson`, `NetworkCompany`, `CoDirectorCompany`, `CoDirector`, `NetworkStats`, `PersonNetworkResponse`, `BoardMemberParams`
- Enriched watchlist: `WatchlistCompanyEntry`

**Enrichment provenance fields** (all optional, backwards-compatible — populated by backend enrichment pipelines):

- `Company.DirectParentLei`, `UltimateParentLei`, `UltimateParentName`, `GleifParentEnrichedAt` — GLEIF parent linkage
- `Company.IndustrySource`, `IndustryConfidence`, `IndustryClassifiedAt` — industry-classification provenance
- `Classification.IndustrySource`, `IndustryConfidence` — same on the classification endpoint
- `Fingerprint.RegistrationDate` — Swiss register entry date
- `BoardMember.RoleSource`, `RoleConfidence`, `RoleInferredAt` — role extraction provenance
- `PersonRoleDetail`, `PersonEntry`, `NetworkCompany` — same role provenance fields

**Data-coverage disclosure:**

- `UboResponse.DataCoverageNote` — explains when chain is incomplete
- `FlowsResponse.DataCoverageNote` — surfaces asymmetric-accuracy notes (e.g. historical dissolution under-counting)

**Documentation:**

- Non-Swiss GLEIF parents appear as synthetic `LEI:{20-char-lei}` identifiers on
  `UboPerson.ControllingEntityUid`, `ChainLink.FromUid`/`ToUid`, and
  `OwnershipLink.SourceUid`/`TargetUid`. Documented on each class.

### Changed

- `HierarchyResponse.Parent`/`Subsidiaries`/`Siblings` now use the typed `HierarchyEntity` class instead of `JsonElement` — typed-output improvement; callers that used `.GetProperty("name")` must switch to field access (`entity.Name`).
- `WatchlistCompaniesResponse` now includes a `Companies: List<WatchlistCompanyEntry>` field alongside the existing `Uids`. The server populates it with name/status/canton for each watched company (eliminates an extra round trip).

### Deprecated

- `Companies.ExportExcelAsync(request)` — marked with `[Obsolete]` attribute. Kept as an alias for `ExportCsvAsync` (the endpoint has always returned CSV; the new name reflects reality). Will be removed in v4.0.

## [3.0.0] - 2026-04-08

Major release aligned with the Rust SDK v2.2.0 and the production VynCo API.

### Added

- **Companies resource** — `GetFullAsync` (full company with persons/changes/relationships), `ClassificationAsync` (industry classification), `StructureAsync` (corporate structure), `AcquisitionsAsync`, `NotesAsync`, `CreateNoteAsync`, `UpdateNoteAsync`, `DeleteNoteAsync`, `TagsAsync`, `CreateTagAsync`, `DeleteTagAsync`, `AllTagsAsync`, `ExportExcelAsync` — 13 new methods
- **Persons resource** — `SearchAsync` (paged person search), `GetAsync` (detailed person record with roles) — 2 new methods
- **Dossiers resource** — `GenerateAsync` (generate dossier for a company by UID)
- **Teams resource** — `JoinAsync` (join a team via invitation token)
- **CompanyListParams** — new filter fields: `Status`, `LegalForm`, `CapitalMin`, `CapitalMax`, `AuditorCategory`, `SortBy`, `SortDesc`
- **Company model** — expanded to 39 fields matching the API: `Currency`, `Purpose`, `FoundingDate`, `RegistrationDate`, `DeletionDate`, `LegalSeat`, `Municipality`, `DataSource`, `EnrichmentLevel`, `AddressStreet`, `AddressHouseNumber`, `AddressZipCode`, `AddressCity`, `AddressCanton`, `Website`, `SubIndustry`, `EmployeeCount`, `AuditorName`, `Latitude`, `Longitude`, `GeoPrecision`, `NogaCode`, `SanctionsHit`, `LastScreenedAt`, `IsFinmaRegulated`, `Ehraid`, `Chid`, `CantonalExcerptUrl`, `OldNames`, `Translations`
- New models: `CompanyFullResponse`, `PersonEntry`, `ChangeEntry`, `RelationshipEntry`, `Classification`, `CorporateStructure`, `RelatedCompanyEntry`, `Acquisition`, `Note`, `CreateNoteRequest`, `UpdateNoteRequest`, `Tag`, `CreateTagRequest`, `TagSummary`, `ExcelExportRequest`, `ExcelExportFilter`, `PersonSearchParams`, `PersonSearchResult`, `PersonDetail`, `PersonRoleDetail`, `JoinTeamRequest`, `JoinTeamResponse`, `LongestTenure`
- Retry logic now falls back to `X-RateLimit-Reset` header when `Retry-After` is absent, capped at 60 seconds
- `RequestBytesAsync` now supports POST with body (for Excel export)

### Changed

- **BREAKING:** Default base URL changed from `https://api.vynco.ch` to `https://vynco.ch/api` (production deployment)
- **BREAKING:** `DataCompleteness` model — fields replaced: `WithCanton`/`WithStatus`/`WithLegalForm`/`WithCapital`/`WithIndustry`/`WithAuditor`/`CompletenessPct` → `EnrichedCompanies`/`CompaniesWithIndustry`/`CompaniesWithGeo`/`TotalPersons`/`TotalChanges`/`TotalSogcPublications`
- **BREAKING:** `PipelineStatus` model — fields replaced: `Name`/`LastRun`/`RecordsProcessed`/`DurationSeconds` → `Id`/`ItemsProcessed`/`LastCompletedAt`
- **BREAKING:** `AuditorTenureStats` model — fields replaced: `TotalTenures`/`LongTenures7Plus`/`MaxTenureYears` → `TotalTracked`/`CurrentAuditors`/`TenuresOver10Years`/`TenuresOver7Years`/`LongestTenure`
- **BREAKING:** `AiSearchResponse.Results` changed from `List<Company>` to `List<JsonElement>` (matching API response)
- **BREAKING:** `CreditBalance.UsedThisMonth` changed from `int` to `long`
- **BREAKING:** `BillingSummary.UsedThisMonth` changed from `int` to `long`
- **BREAKING:** `MemberUsage.CreditsUsed` changed from `int` to `long`
- SDK user-agent updated to `vynco-dotnet/3.0.0`
- Package version bumped to 3.0.0
- Total endpoints: 69 → 86

## [2.0.0] - 2026-03-31

Major release aligned with the VynCo API v2 (`/v1`). All resources, DTOs, and endpoints updated to match the Rust SDK v2.0.0.

### Added

- **Auditors resource** — `HistoryAsync`, `TenuresAsync` for auditor tenure tracking
- **Dashboard resource** — `GetAsync` for admin dashboard with data completeness and pipeline status
- **Screening resource** — `ScreenAsync` for compliance/sanctions screening
- **Watchlists resource** — `ListAsync`, `CreateAsync`, `DeleteAsync`, `CompaniesAsync`, `AddCompaniesAsync`, `RemoveCompanyAsync`, `EventsAsync` for managing company watchlists
- **Webhooks resource** — `ListAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `TestAsync`, `DeliveriesAsync` for webhook subscriptions
- **Exports resource** — `CreateAsync`, `GetAsync`, `DownloadAsync` for bulk data exports (including binary file download)
- **AI resource** — `DossierAsync`, `SearchAsync`, `RiskScoreAsync` for AI-powered analysis
- **Graph resource** — `GetAsync`, `ExportAsync`, `AnalyzeAsync` for network graphs and analysis
- **Companies resource** — `EventsAsync`, `FingerprintAsync`, `NearbyAsync` new methods
- **Analytics resource** — `CandidatesAsync` for audit candidate listing
- Response headers: `RateLimitRemaining`, `RateLimitReset` fields added to `VynCoResponseHeaders`
- `RequestBytesAsync` internal method for binary file downloads (`ExportFile` type)
- New models: `AuditorHistoryResponse`, `AuditorTenure`, `AuditorTenureParams`, `DashboardResponse`, `DataCompleteness`, `PipelineStatus`, `AuditorTenureStats`, `ScreeningRequest`, `ScreeningResponse`, `ScreeningHit`, `Watchlist`, `WatchlistSummary`, `CreateWatchlistRequest`, `WatchlistCompaniesResponse`, `AddCompaniesRequest`, `AddCompaniesResponse`, `WebhookSubscription`, `CreateWebhookRequest`, `CreateWebhookResponse`, `UpdateWebhookRequest`, `TestDeliveryResponse`, `WebhookDelivery`, `CreateExportRequest`, `ExportJob`, `ExportDownload`, `ExportFile`, `AiDossierRequest`, `AiDossierResponse`, `AiSearchRequest`, `AiSearchResponse`, `RiskScoreRequest`, `RiskScoreResponse`, `RiskFactor`, `GraphResponse`, `GraphNode`, `GraphLink`, `NetworkAnalysisRequest`, `NetworkAnalysisResponse`, `NetworkCluster`, `EventListResponse`, `CompanyEvent`, `CompareRequest`, `CompareResponse`, `ComparisonDimension`, `NewsItem`, `CompanyReport`, `Relationship`, `HierarchyResponse`, `Fingerprint`, `NearbyParams`, `NearbyCompany`, `Invitation`, `DossierSummary`, `CantonDistribution`, `AuditorMarketShare`, `ClusterResponse`, `ClusterResult`, `AnomalyResponse`, `RfmSegmentsResponse`, `RfmSegment`, `CohortResponse`, `CohortEntry`, `CreditUsage`, `UsagePeriod`, `UsageRow`, `CreditHistory`, `CheckoutRequest`, `AuditCandidate`, `CandidateParams`

### Changed

- **BREAKING:** API path prefix changed from `/api/v1/` to `/v1/`
- **BREAKING:** Health endpoint path changed from `/api/v1/health` to `/health`
- **BREAKING:** `Watches` resource replaced by `Watchlists` resource with full CRUD
- **BREAKING:** `News` resource removed — news is now available via `Companies.NewsAsync()`
- **BREAKING:** `SyncStatus` resource removed
- **BREAKING:** `Companies` resource — removed `SearchAsync`, `BatchAsync`; `CompareAsync` now returns `CompareResponse` (was `JsonElement`); `RelationshipsAsync` returns `List<Relationship>` (was `RelationshipResponse`); `HierarchyAsync` returns `HierarchyResponse` (was `RelationshipResponse`); `NewsAsync` returns `List<NewsItem>` (was `CompanyNewsResponse`); `ReportsAsync` returns `List<CompanyReport>` (was `CompanyReportsResponse`)
- **BREAKING:** `Changes` resource — removed `GetBySogcIdAsync`, `ReviewAsync`, `BatchAsync`; `ByCompanyAsync` replaces `GetByCompanyAsync`; `CompanyChange` replaces `Change` model with different fields
- **BREAKING:** `Persons` resource — stripped to single method `BoardMembersAsync`; removed `ListAsync`, `GetAsync`, `GetRolesAsync`, `GetConnectionsAsync`, `NetworkStatsAsync`; `BoardMember` model now has flat fields (was nested `Person`/`PersonConnection`)
- **BREAKING:** `Dossiers` resource — `CreateAsync` replaces `GenerateAsync`; returns new `Dossier` model; `ListAsync` returns `List<DossierSummary>`; `DeleteAsync` added
- **BREAKING:** `Analytics` resource — removed `VelocityAsync`, `DetectAnomaliesAsync`; all methods now return strongly typed models instead of `JsonElement`; `ClusterAsync`/`AnomaliesAsync` use new request types (`ClusterRequest`, `AnomalyRequest`)
- **BREAKING:** `Teams` resource — `MeAsync` replaces `GetCurrentAsync`; `MembersAsync` replaces `ListMembersAsync`; `InviteMemberAsync` returns `Invitation` (was `TeamMember`); member IDs now `string` (was `Guid`); `Team`/`TeamMember`/`BillingSummary`/`MemberUsage` models simplified
- **BREAKING:** `Billing` resource — `CreateCheckoutAsync` replaces `CreateCheckoutSessionAsync`; `CreatePortalAsync` replaces `CreatePortalSessionAsync`; `CheckoutRequest` replaces `CheckoutSessionRequest`
- **BREAKING:** `ApiKeys` resource — key IDs now `string` (was `Guid`); `CreateApiKeyRequest` now uses `Environment`/`Scopes` (was `Name`/`IsTestKey`)
- **BREAKING:** `Credits` resource — `UsageAsync` returns `CreditUsage` (was `UsageBreakdown`); `HistoryAsync` returns `CreditHistory` (was `List<CreditLedgerEntry>`); `CreditBalance.OverageRate` is `double` (was `decimal`)
- **BREAKING:** `PagedResponse<T>` — uses `Total`/`Page`/`PageSize` fields (removed `TotalCount`, `TotalPages`, `HasPreviousPage`, `HasNextPage`)
- **BREAKING:** `Company` model simplified — removed `LegalFormId`, `Purpose`, `Address`, `RegisteredAddress`, `Currency`, `CurrentAuditor`, `IsActive`, `FoundingDate`, `DeletionDate`, `CreatedAt`; `ShareCapital` is `double?` (was `decimal?`); added `Industry`, `UpdatedAt` (string)
- **BREAKING:** `HealthResponse` model changed — now has `Database`, `Redis`, `Version` fields (was `Uptime`, `Checks`)
- **BREAKING:** `ChangeStatistics` — new field names (`TotalChanges`, `ChangesThisWeek`, `ChangesThisMonth`, `ByType` as `JsonElement`)
- **BREAKING:** Response header names updated — `X-RateLimit-Limit`/`X-RateLimit-Remaining`/`X-RateLimit-Reset` (was `X-Rate-Limit-Limit`)
- Removed `BatchLookupRequest` model
- SDK user-agent updated to `vynco-dotnet/2.0.0`
- Package description updated
- Package version bumped to 2.0.0

### Removed

- `WatchesResource`, `NewsResource`, `SyncStatusResource` and their models
- `CompanySearchRequest`, `CompanyCompareRequest`, `BatchLookupRequest`, `CompanyRelationship`, `RelationshipResponse`, `CompanyNewsResponse`, `CompanyReportsResponse`, `ReviewChangeRequest`, `ReviewResult`, `ListChangesParams`, `Person`, `PersonConnection`, `ListPersonsParams`, `DossierStatistics`, `GenerateDossierRequest`, `ClusteringRequest`, `AnomalyDetectionRequest`, `CohortAnalyticsParams`, `VelocityParams`, `CheckoutSessionRequest`, `UsageBreakdown`, `OperationUsage`, `CreditUsageParams`, `CreditHistoryParams`, `CompanyWatch`, `AddWatchRequest`, `ChangeNotification`, `RecentNewsResponse`, `SyncStatusEntry`, `HealthCheck`, `ListCompaniesParams`

## [1.0.0] - 2026-03-18

First stable release aligned with the finalized VynCo API v1 (`/api/v1`).

### Added

- **Response headers** — `client.LastResponseHeaders` exposes `X-Request-Id`, `X-Credits-Used`, `X-Credits-Remaining`, `X-Rate-Limit-Limit`, `X-Data-Source`, and `Retry-After` from every API response
- **Analytics resource** — `ClusterAsync`, `DetectAnomaliesAsync`, `CohortsAsync`, `CantonsAsync`, `AuditorsAsync`, `RfmSegmentsAsync`, `VelocityAsync`
- **Watches resource** — `ListAsync`, `AddAsync`, `RemoveAsync`, `ListNotificationsAsync` for company change subscriptions and notifications
- **Health resource** — `CheckAsync` for API health checks (no auth required)
- **News resource** — `GetRecentAsync` for recent news across all companies
- **Companies resource** — `CompareAsync`, `GetRelationshipsAsync`, `GetHierarchyAsync`, `GetNewsAsync`, `GetReportsAsync`
- **Changes resource** — `GetBySogcIdAsync` for SOGC publication lookup
- **Persons resource** — `ListAsync` (paged), `GetConnectionsAsync`, `NetworkStatsAsync`
- **Teams resource** — `ListMembersAsync`, `InviteMemberAsync`, `UpdateMemberRoleAsync`, `RemoveMemberAsync`, `BillingSummaryAsync`
- **Dossiers resource** — `GenerateAsync` now accepts optional `GenerateDossierRequest` with `Type` (standard/comprehensive)
- **Billing resource** — `CreateCheckoutSessionAsync` now accepts optional `CheckoutSessionRequest` with target `Tier`
- New exception types: `BadRequestException` (400), `ForbiddenException` (403)
- New models: `CompanyRelationship`, `RelationshipResponse`, `CompanyWatch`, `AddWatchRequest`, `ChangeNotification`, `TeamMember`, `InviteMemberRequest`, `UpdateMemberRoleRequest`, `BillingSummary`, `MemberUsage`, `HealthResponse`, `HealthCheck`, `ClusteringRequest`, `AnomalyDetectionRequest`, `GenerateDossierRequest`, `CheckoutSessionRequest`, `RecentNewsResponse`, `CompanyNewsResponse`, `CompanyReportsResponse`, `CompanyCompareRequest`
- `Company` model: added `Address` field
- `Change` model: added `CompanyName`, `SogcId` fields
- `Person` model: added `Name`, `Roles`, `Companies` fields (simplified API form alongside detailed fields)
- `Dossier` model: added `CompanyName`, `Summary`, `RiskScore` fields
- `ApiKey` model: added `Prefix`, `IsTestKey` fields; `ApiKeyCreated` adds `Key` field
- `VynCoClient.SdkVersion` public constant

### Changed

- **BREAKING:** API path prefix changed from `/v1/` to `/api/v1/` to match the finalized OpenAPI spec
- **BREAKING:** `Webhooks` resource replaced by `Watches` resource (API uses company watches, not webhooks)
- **BREAKING:** `Persons.SearchAsync` replaced by `Persons.ListAsync` with `ListPersonsParams` (paged, uses `search` query param)
- **BREAKING:** `Persons.GetBoardMembersAsync` now returns `List<Person>` instead of `List<BoardMember>` (matching the OpenAPI spec)
- **BREAKING:** `Dossiers.GenerateAsync` now returns `Dossier` instead of `DossierGenerated`; accepts optional `GenerateDossierRequest`
- **BREAKING:** `Changes.ReviewChangeRequest` simplified to only `ReviewNotes` (matching the finalized API)
- **BREAKING:** `Companies.CompanySearchRequest.Limit` default changed from 50 to 25
- **BREAKING:** `Companies.BatchAsync` max changed from 500 to 50 UIDs
- `Company.AuditorCategory` changed from non-nullable `string` to `string?`
- SDK user-agent updated to `vynco-dotnet/1.0.0`
- Package version bumped to 1.0.0

### Removed

- `Webhooks` resource and models (`Webhook`, `WebhookCreated`, `CreateWebhookRequest`)
- `DossierGenerated` model (replaced by returning `Dossier` directly)
- `SearchPersonsParams` (replaced by `ListPersonsParams`)
- `CreateApiKeyRequest.Permissions` field (not in finalized API)

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
- `BatchLookupRequest` for batch operations
- Multi-targeting: `netstandard2.0` and `net10.0`

[3.0.0]: https://github.com/VynCorp/vc-dotnet/compare/v2.0.0...v3.0.0
[2.0.0]: https://github.com/VynCorp/vc-dotnet/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/VynCorp/vc-dotnet/compare/v0.1.0...v1.0.0
[0.1.0]: https://github.com/VynCorp/vc-dotnet/releases/tag/v0.1.0
