# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Test Commands

```bash
dotnet build                          # Build all projects (both TFMs)
dotnet test                           # Run all tests
dotnet test --filter "FullyQualifiedName~MethodName"  # Run a single test
dotnet pack -c Release                # Create NuGet package
```

Tests target `net10.0` only. The SDK targets `netstandard2.0` and `net10.0` — both TFMs must compile cleanly with zero warnings.

## Architecture

This is a .NET SDK wrapping the VynCo Swiss Corporate Intelligence REST API (`https://api.vynco.ch/v1/*`). It follows the same resource-based pattern as the [Rust SDK](https://github.com/VynCorp/vc-rust).

**Core pattern:** `VynCoClient` owns an `HttpClient` and exposes 18 resource properties. Each resource class holds a reference to the client and delegates HTTP calls to `VynCoClient.RequestAsync<T>()` internal methods, which handle serialization, retry, error mapping, and response header capture.

```
VynCoClient (entry point, IDisposable)
├── RequestAsync<T>()          — JSON request/response with retry
├── RequestVoidAsync()         — for 204 No Content
├── RequestListAsync<T>()      — extracts arrays from various response shapes
├── RequestBytesAsync()        — binary file downloads (ExportFile)
├── CaptureResponseHeaders()   — extracts X-Credits-Used, X-Request-Id, etc.
├── MapException()             — maps HTTP status → typed exception (RFC 7807)
├── LastResponseHeaders        — headers from the most recent API response
└── 18 Resource properties:
    Health, Companies, Auditors, Dashboard, Screening,
    Watchlists, Webhooks, Exports, Ai, ApiKeys,
    Credits, Billing, Teams, Changes, Persons,
    Analytics, Dossiers, Graph
```

**Key conventions:**
- All DTOs use explicit `[JsonPropertyName("camelCase")]` — the backend uses camelCase (not snake_case)
- `PropertyNamingPolicy = null` in serializer options — never rely on automatic naming
- Error responses are RFC 7807 `ProblemDetails`
- Retry with exponential backoff on 429 and 5xx: `500ms * 2^attempt`
- `ConfigureAwait(false)` on all awaits (library code)
- `#if NET8_0_OR_GREATER` guards on `ReadAsStringAsync(ct)` and `ReadAsByteArrayAsync(ct)` overloads for netstandard2.0 compat
- Response headers (`X-Credits-Used`, `X-Credits-Remaining`, `X-Request-Id`, `X-RateLimit-Limit`, `X-RateLimit-Remaining`, `X-RateLimit-Reset`, `X-Data-Source`) captured into `LastResponseHeaders` after every request
- API paths use `/v1/` prefix (except `/health` which has no prefix)
- All IDs and timestamps use `string` type (matching the Rust SDK)

**Testing pattern:** `MockHttpHandler` captures requests; `TestHelper.CreateClient()` injects it into `VynCoClient` via reflection on the private `_http` field. Tests always set `maxRetries: 0` to avoid retry delays. The `MockHttpHandler` supports both simple (status+body) and callback-based construction for testing response headers.

## Backend Reference

The API backend is the ZefixMiner Azure Functions project at `/home/michael/DEV/Repos/ZefixMiner/EY.EW.ASU.ZefixMiner`. The frontend portal is at `/home/michael/DEV/Repos/VynCorpPortal/VynCorpPortal`. API contracts (request/response DTOs, endpoint routes) live in `ZefixMiner.Functions.Api/Contracts/` and `ZefixMiner.Functions.Api/Functions/`. The authoritative OpenAPI spec is at `ZefixMiner.Functions.Api/openapi.json`. The Rust SDK reference is at `/home/michael/DEV/Repos/VynCorp-rust/vc-rust`.
