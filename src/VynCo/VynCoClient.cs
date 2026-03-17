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

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>Companies resource — search, list, get, count, statistics, batch lookup.</summary>
    public CompaniesResource Companies { get; }
    /// <summary>Changes resource — list, get by company, statistics, review, batch.</summary>
    public ChangesResource Changes { get; }
    /// <summary>Persons resource — search, get, roles, board members.</summary>
    public PersonsResource Persons { get; }
    /// <summary>Dossiers resource — generate, get, list AI-powered company dossiers.</summary>
    public DossiersResource Dossiers { get; }
    /// <summary>API key management resource.</summary>
    public ApiKeysResource ApiKeys { get; }
    /// <summary>Credits resource — balance, usage, history.</summary>
    public CreditsResource Credits { get; }
    /// <summary>Billing resource — Stripe checkout and portal sessions.</summary>
    public BillingResource Billing { get; }
    /// <summary>Teams resource — create and get team.</summary>
    public TeamsResource Teams { get; }
    /// <summary>Webhooks resource — CRUD and testing.</summary>
    public WebhooksResource Webhooks { get; }
    /// <summary>Sync status resource — registry sync status.</summary>
    public SyncStatusResource SyncStatus { get; }

    public VynCoClient(string apiKey, string baseUrl = "https://api.vynco.ch", int maxRetries = 2, TimeSpan? timeout = null)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key is required.", nameof(apiKey));

        _baseUrl = baseUrl.TrimEnd('/');
        _maxRetries = maxRetries;

        _http = new HttpClient { Timeout = timeout ?? TimeSpan.FromSeconds(30) };
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("vynco-dotnet/0.1.0");
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        Companies = new CompaniesResource(this);
        Changes = new ChangesResource(this);
        Persons = new PersonsResource(this);
        Dossiers = new DossiersResource(this);
        ApiKeys = new ApiKeysResource(this);
        Credits = new CreditsResource(this);
        Billing = new BillingResource(this);
        Teams = new TeamsResource(this);
        Webhooks = new WebhooksResource(this);
        SyncStatus = new SyncStatusResource(this);
    }

    // -- Internal request methods used by resources --

    internal async Task<T> RequestAsync<T>(HttpMethod method, string path, CancellationToken ct = default)
    {
        return await RequestAsync<T>(method, path, body: (object?)null, ct).ConfigureAwait(false);
    }

    internal async Task<T> RequestAsync<T>(HttpMethod method, string path, object? body, CancellationToken ct = default)
    {
        var url = $"{_baseUrl}{path}";
        Exception? lastException = null;

        for (int attempt = 0; attempt <= _maxRetries; attempt++)
        {
            using var request = new HttpRequestMessage(method, url);
            if (body is not null)
            {
                var json = JsonSerializer.Serialize(body, JsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
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
                    await Task.Delay(Backoff(attempt), ct).ConfigureAwait(false);
                    continue;
                }
                throw new VynCoException($"HTTP request failed: {ex.Message}");
            }

            if (ShouldRetry(response.StatusCode) && attempt < _maxRetries)
            {
                response.Dispose();
                await Task.Delay(Backoff(attempt), ct).ConfigureAwait(false);
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

    internal static JsonSerializerOptions GetJsonOptions() => JsonOptions;

    private static List<T> ExtractList<T>(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Array)
            return JsonSerializer.Deserialize<List<T>>(value.GetRawText(), JsonOptions) ?? new();

        if (value.ValueKind == JsonValueKind.Object)
        {
            if (value.TryGetProperty("data", out var dataArr) && dataArr.ValueKind == JsonValueKind.Array)
                return JsonSerializer.Deserialize<List<T>>(dataArr.GetRawText(), JsonOptions) ?? new();

            foreach (var prop in value.EnumerateObject())
            {
                if (prop.Value.ValueKind == JsonValueKind.Array)
                    return JsonSerializer.Deserialize<List<T>>(prop.Value.GetRawText(), JsonOptions) ?? new();
            }
        }

        return new();
    }

    private static bool ShouldRetry(HttpStatusCode status)
        => status == (HttpStatusCode)429 || (int)status >= 500;

    private static TimeSpan Backoff(int attempt)
        => TimeSpan.FromMilliseconds(500 * Math.Pow(2, attempt));

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
            HttpStatusCode.Unauthorized => new AuthenticationException(message, problemDetails),
            HttpStatusCode.PaymentRequired => new InsufficientCreditsException(message, problemDetails),
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
