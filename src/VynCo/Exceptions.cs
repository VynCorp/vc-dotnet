using System.Text.Json.Serialization;

namespace VynCo;

/// <summary>Base exception for all VynCo API errors.</summary>
public class VynCoException : Exception
{
    public int? StatusCode { get; }
    public ProblemDetails? Body { get; }

    public VynCoException(string message, int? statusCode = null, ProblemDetails? body = null)
        : base(message)
    {
        StatusCode = statusCode;
        Body = body;
    }
}

public class BadRequestException : VynCoException
{
    public BadRequestException(string message, ProblemDetails? body = null)
        : base(message, 400, body) { }
}

public class AuthenticationException : VynCoException
{
    public AuthenticationException(string message, ProblemDetails? body = null)
        : base(message, 401, body) { }
}

public class InsufficientCreditsException : VynCoException
{
    public InsufficientCreditsException(string message, ProblemDetails? body = null)
        : base(message, 402, body) { }
}

public class ForbiddenException : VynCoException
{
    public ForbiddenException(string message, ProblemDetails? body = null)
        : base(message, 403, body) { }
}

public class NotFoundException : VynCoException
{
    public NotFoundException(string message, ProblemDetails? body = null)
        : base(message, 404, body) { }
}

public class ConflictException : VynCoException
{
    public ConflictException(string message, ProblemDetails? body = null)
        : base(message, 409, body) { }
}

public class ValidationException : VynCoException
{
    public ValidationException(string message, ProblemDetails? body = null)
        : base(message, 422, body) { }
}

public class RateLimitException : VynCoException
{
    public RateLimitException(string message, ProblemDetails? body = null)
        : base(message, 429, body) { }
}

public class ServerException : VynCoException
{
    public ServerException(string message, ProblemDetails? body = null)
        : base(message, 500, body) { }
}

/// <summary>RFC 7807 Problem Details response from the VynCo API.</summary>
public class ProblemDetails
{
    [JsonPropertyName("type")] public string Type { get; set; } = "";
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("status")] public int Status { get; set; }
    [JsonPropertyName("detail")] public string? Detail { get; set; }
    [JsonPropertyName("instance")] public string? Instance { get; set; }
    [JsonPropertyName("traceId")] public string? TraceId { get; set; }
}

/// <summary>Response headers returned by the VynCo API on every request.</summary>
public class VynCoResponseHeaders
{
    /// <summary>Unique request identifier for support and debugging.</summary>
    public string? RequestId { get; set; }

    /// <summary>Credits debited for the completed operation.</summary>
    public int? CreditsUsed { get; set; }

    /// <summary>Credit balance remaining after the operation.</summary>
    public int? CreditsRemaining { get; set; }

    /// <summary>Rate limit ceiling for the current tier (requests per minute).</summary>
    public int? RateLimitLimit { get; set; }

    /// <summary>Data source attribution header.</summary>
    public string? DataSource { get; set; }

    /// <summary>Seconds to wait before retrying (present on 429 responses).</summary>
    public int? RetryAfter { get; set; }
}
