using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VynCo.Models;
using VynCo.Resources;

namespace VynCo;

/// <summary>VynCo API client for Swiss corporate intelligence data.</summary>
public class VynCoClient : IDisposable
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;
    private readonly int _maxRetries;
    private bool _disposed;

    public const string SdkVersion = "3.2.0";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    // -- Resource properties --

    /// <summary>Health resource — API health check.</summary>
    public HealthResource Health { get; }
    /// <summary>Companies resource — list, get, full, count, events, statistics, compare, news, reports, relationships, hierarchy, classification, fingerprint, structure, acquisitions, nearby, notes, tags, excel export.</summary>
    public CompaniesResource Companies { get; }
    /// <summary>Auditors resource — auditor history and tenures.</summary>
    public AuditorsResource Auditors { get; }
    /// <summary>Dashboard resource — admin dashboard.</summary>
    public DashboardResource Dashboard { get; }
    /// <summary>Screening resource — compliance screening.</summary>
    public ScreeningResource Screening { get; }
    /// <summary>Watchlists resource — manage watchlists and their companies.</summary>
    public WatchlistsResource Watchlists { get; }
    /// <summary>Webhooks resource — manage webhook subscriptions.</summary>
    public WebhooksResource Webhooks { get; }
    /// <summary>Exports resource — create, get, and download data exports.</summary>
    public ExportsResource Exports { get; }
    /// <summary>AI resource — dossier generation, search, risk scoring.</summary>
    public AiResource Ai { get; }
    /// <summary>API key management resource.</summary>
    public ApiKeysResource ApiKeys { get; }
    /// <summary>Credits resource — balance, usage, history.</summary>
    public CreditsResource Credits { get; }
    /// <summary>Billing resource — Stripe checkout and portal sessions.</summary>
    public BillingResource Billing { get; }
    /// <summary>Teams resource — create, get, manage members, billing summary, join.</summary>
    public TeamsResource Teams { get; }
    /// <summary>Changes resource — list, get by company, statistics.</summary>
    public ChangesResource Changes { get; }
    /// <summary>Persons resource — board members, search, get details.</summary>
    public PersonsResource Persons { get; }
    /// <summary>Analytics resource — cantons, auditors, clustering, anomalies, RFM, cohorts, candidates.</summary>
    public AnalyticsResource Analytics { get; }
    /// <summary>Dossiers resource — create, list, get, delete, generate managed dossiers.</summary>
    public DossiersResource Dossiers { get; }
    /// <summary>Graph resource — network graphs and analysis.</summary>
    public GraphResource Graph { get; }
    /// <summary>Alerts resource — saved queries with optional webhook delivery (v3.1+).</summary>
    public AlertsResource Alerts { get; }
    /// <summary>Ownership resource — ownership-chain trace with circular-ownership detection (v3.1+).</summary>
    public OwnershipResource Ownership { get; }
    /// <summary>Reports resource — industry reports and AI-generated narratives.</summary>
    public ReportsResource Reports { get; }
    /// <summary>Pipelines resource — sales/prospect tracking pipelines.</summary>
    public PipelinesResource Pipelines { get; }
    /// <summary>Saved searches resource — manage persistent search queries.</summary>
    public SavedSearchesResource SavedSearches { get; }

    /// <summary>Headers from the most recent API response (request-id, credits, rate-limit).</summary>
    public VynCoResponseHeaders? LastResponseHeaders { get; private set; }

    public VynCoClient(string apiKey, string baseUrl = "https://vynco.ch/api", int maxRetries = 2, TimeSpan? timeout = null)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key is required.", nameof(apiKey));

        _baseUrl = baseUrl.TrimEnd('/');
        _maxRetries = maxRetries;

        _http = new HttpClient { Timeout = timeout ?? TimeSpan.FromSeconds(30) };
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _http.DefaultRequestHeaders.UserAgent.ParseAdd($"vynco-dotnet/{SdkVersion}");
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        Health = new HealthResource(this);
        Companies = new CompaniesResource(this);
        Auditors = new AuditorsResource(this);
        Dashboard = new DashboardResource(this);
        Screening = new ScreeningResource(this);
        Watchlists = new WatchlistsResource(this);
        Webhooks = new WebhooksResource(this);
        Exports = new ExportsResource(this);
        Ai = new AiResource(this);
        ApiKeys = new ApiKeysResource(this);
        Credits = new CreditsResource(this);
        Billing = new BillingResource(this);
        Teams = new TeamsResource(this);
        Changes = new ChangesResource(this);
        Persons = new PersonsResource(this);
        Analytics = new AnalyticsResource(this);
        Dossiers = new DossiersResource(this);
        Graph = new GraphResource(this);
        Alerts = new AlertsResource(this);
        Ownership = new OwnershipResource(this);
        Reports = new ReportsResource(this);
        Pipelines = new PipelinesResource(this);
        SavedSearches = new SavedSearchesResource(this);
    }

    // -- Internal request methods used by resources --

    internal async Task<T> RequestAsync<T>(HttpMethod method, string path, CancellationToken ct = default)
    {
        return await RequestAsync<T>(method, path, body: (object?)null, ct).ConfigureAwait(false);
    }

    internal async Task<T> RequestAsync<T>(HttpMethod method, string path, object? body, CancellationToken ct = default)
    {
        var url = $"{_baseUrl}{path}";
        var bodyJson = body is not null ? JsonSerializer.Serialize(body, JsonOptions) : null;
        Exception? lastException = null;

        for (int attempt = 0; attempt <= _maxRetries; attempt++)
        {
            using var request = new HttpRequestMessage(method, url);
            if (bodyJson is not null)
            {
                request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response;
            try
            {
                response = await _http.SendAsync(request, ct).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                lastException = ex;
                if (attempt < _maxRetries)
                {
                    await Task.Delay(GetRetryDelay(attempt, null), ct).ConfigureAwait(false);
                    continue;
                }
                throw new VynCoException($"HTTP request failed: {ex.Message}");
            }

            CaptureResponseHeaders(response);

            if (ShouldRetry(response.StatusCode) && attempt < _maxRetries)
            {
                var delay = GetRetryDelay(attempt, LastResponseHeaders);
                response.Dispose();
                await Task.Delay(delay, ct).ConfigureAwait(false);
                continue;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync(
#if NET8_0_OR_GREATER
                    ct
#endif
                ).ConfigureAwait(false);
                response.Dispose();
                throw MapException(response.StatusCode, errorJson);
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                response.Dispose();
                return default!;
            }

            var responseJson = await response.Content.ReadAsStringAsync(
#if NET8_0_OR_GREATER
                ct
#endif
            ).ConfigureAwait(false);
            response.Dispose();
            return JsonSerializer.Deserialize<T>(responseJson, JsonOptions)!;
        }

        throw new VynCoException("Max retries exceeded", body: null);
    }

    internal async Task RequestVoidAsync(HttpMethod method, string path, CancellationToken ct = default)
    {
        await RequestAsync<object?>(method, path, body: (object?)null, ct).ConfigureAwait(false);
    }

    internal async Task<List<T>> RequestListAsync<T>(HttpMethod method, string path, CancellationToken ct = default)
    {
        var value = await RequestAsync<JsonElement>(method, path, ct).ConfigureAwait(false);
        return ExtractList<T>(value);
    }

    internal async Task<List<T>> RequestListAsync<T>(HttpMethod method, string path, object? body, CancellationToken ct = default)
    {
        var value = await RequestAsync<JsonElement>(method, path, body, ct).ConfigureAwait(false);
        return ExtractList<T>(value);
    }

    internal Task<ExportFile> RequestBytesAsync(string path, CancellationToken ct = default)
        => RequestBytesAsync(path, body: null, ct);

    internal async Task<ExportFile> RequestBytesAsync(string path, object? body, CancellationToken ct = default)
    {
        var url = $"{_baseUrl}{path}";
        var method = body is not null ? HttpMethod.Post : HttpMethod.Get;
        var bodyJson = body is not null ? JsonSerializer.Serialize(body, JsonOptions) : null;

        for (int attempt = 0; attempt <= _maxRetries; attempt++)
        {
            using var request = new HttpRequestMessage(method, url);
            if (bodyJson is not null)
            {
                request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response;
            try
            {
                response = await _http.SendAsync(request, ct).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                if (attempt < _maxRetries)
                {
                    await Task.Delay(GetRetryDelay(attempt, null), ct).ConfigureAwait(false);
                    continue;
                }
                throw new VynCoException($"HTTP request failed: {ex.Message}");
            }

            CaptureResponseHeaders(response);

            if (ShouldRetry(response.StatusCode) && attempt < _maxRetries)
            {
                var delay = GetRetryDelay(attempt, LastResponseHeaders);
                response.Dispose();
                await Task.Delay(delay, ct).ConfigureAwait(false);
                continue;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync(
#if NET8_0_OR_GREATER
                    ct
#endif
                ).ConfigureAwait(false);
                response.Dispose();
                throw MapException(response.StatusCode, errorJson);
            }

            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
            var filename = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "";
            var bytes = await response.Content.ReadAsByteArrayAsync(
#if NET8_0_OR_GREATER
                ct
#endif
            ).ConfigureAwait(false);
            response.Dispose();

            return new ExportFile
            {
                Headers = LastResponseHeaders ?? new VynCoResponseHeaders(),
                Bytes = bytes,
                ContentType = contentType,
                Filename = filename,
            };
        }

        throw new VynCoException("Max retries exceeded", body: null);
    }

    internal static JsonSerializerOptions GetJsonOptions() => JsonOptions;

    private void CaptureResponseHeaders(HttpResponseMessage response)
    {
        var headers = new VynCoResponseHeaders();

        if (response.Headers.TryGetValues("X-Request-Id", out var reqId))
            headers.RequestId = string.Join(",", reqId);

        if (response.Headers.TryGetValues("X-Credits-Used", out var cu) && int.TryParse(string.Join("", cu), out var creditsUsed))
            headers.CreditsUsed = creditsUsed;

        if (response.Headers.TryGetValues("X-Credits-Remaining", out var cr) && int.TryParse(string.Join("", cr), out var creditsRemaining))
            headers.CreditsRemaining = creditsRemaining;

        if (response.Headers.TryGetValues("X-RateLimit-Limit", out var rl) && int.TryParse(string.Join("", rl), out var rateLimitLimit))
            headers.RateLimitLimit = rateLimitLimit;

        if (response.Headers.TryGetValues("X-RateLimit-Remaining", out var rlr) && int.TryParse(string.Join("", rlr), out var rateLimitRemaining))
            headers.RateLimitRemaining = rateLimitRemaining;

        if (response.Headers.TryGetValues("X-RateLimit-Reset", out var rlre) && long.TryParse(string.Join("", rlre), out var rateLimitReset))
            headers.RateLimitReset = rateLimitReset;

        if (response.Headers.TryGetValues("X-Data-Source", out var ds))
            headers.DataSource = string.Join(",", ds);

        if (response.Headers.TryGetValues("Retry-After", out var ra) && int.TryParse(string.Join("", ra), out var retryAfter))
            headers.RetryAfter = retryAfter;

        LastResponseHeaders = headers;
    }

    private static List<T> ExtractList<T>(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Array)
            return value.Deserialize<List<T>>(JsonOptions) ?? new();

        if (value.ValueKind == JsonValueKind.Object)
        {
            if (value.TryGetProperty("data", out var dataArr) && dataArr.ValueKind == JsonValueKind.Array)
                return dataArr.Deserialize<List<T>>(JsonOptions) ?? new();

            foreach (var prop in value.EnumerateObject())
            {
                if (prop.Value.ValueKind == JsonValueKind.Array)
                    return prop.Value.Deserialize<List<T>>(JsonOptions) ?? new();
            }
        }

        return new();
    }

    private static bool ShouldRetry(HttpStatusCode status)
        => status == (HttpStatusCode)429 || (int)status >= 500;

    private static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(60);

    private static TimeSpan GetRetryDelay(int attempt, VynCoResponseHeaders? headers)
    {
        // Prefer Retry-After header
        if (headers?.RetryAfter is > 0)
            return Clamp(TimeSpan.FromSeconds(headers.RetryAfter.Value));

        // Fall back to X-RateLimit-Reset (unix timestamp)
        if (headers?.RateLimitReset is > 0)
        {
            var resetTime = DateTimeOffset.FromUnixTimeSeconds(headers.RateLimitReset.Value);
            var wait = resetTime - DateTimeOffset.UtcNow;
            if (wait > TimeSpan.Zero)
                return Clamp(wait);
        }

        // Default: exponential backoff 500ms * 2^attempt
        return Clamp(TimeSpan.FromMilliseconds(500 * Math.Pow(2, attempt)));
    }

    private static TimeSpan Clamp(TimeSpan delay)
        => delay > MaxRetryDelay ? MaxRetryDelay : delay;

    private static VynCoException MapException(HttpStatusCode status, string body)
    {
        ProblemDetails? problemDetails = null;
        string message;
        try
        {
            problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body, JsonOptions);
            message = !string.IsNullOrEmpty(problemDetails?.Detail) ? problemDetails!.Detail!
                     : !string.IsNullOrEmpty(problemDetails?.Title) ? problemDetails!.Title
                     : $"HTTP {(int)status}";
        }
        catch
        {
            message = $"HTTP {(int)status}";
        }

        return status switch
        {
            HttpStatusCode.BadRequest => new BadRequestException(message, problemDetails),
            HttpStatusCode.Unauthorized => new AuthenticationException(message, problemDetails),
            HttpStatusCode.PaymentRequired => new InsufficientCreditsException(message, problemDetails),
            HttpStatusCode.Forbidden => new ForbiddenException(message, problemDetails),
            HttpStatusCode.NotFound => new NotFoundException(message, problemDetails),
            HttpStatusCode.Conflict => new ConflictException(message, problemDetails),
            (HttpStatusCode)422 => new ValidationException(message, problemDetails),
            (HttpStatusCode)429 => new RateLimitException(message, problemDetails),
            _ when (int)status >= 500 => new ServerException(message, problemDetails),
            _ => new VynCoException(message, (int)status, problemDetails),
        };
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _http.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
