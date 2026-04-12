using VynCo.Models;

namespace VynCo.Resources;

/// <summary>
/// Ownership trace operations (v3.1+).
///
/// For ultimate beneficial owner resolution use <see cref="CompaniesResource.UboAsync"/> —
/// this resource exposes the lower-level ownership-chain trace endpoint that
/// walks head-office / branch-office / acquisition relationships upward and
/// detects circular ownership.
/// </summary>
public class OwnershipResource
{
    private readonly VynCoClient _client;
    internal OwnershipResource(VynCoClient client) => _client = client;

    /// <summary>
    /// Trace the ownership chain upward from a company.
    ///
    /// Walks head-office / branch-office relationships up to
    /// <paramref name="request"/>.MaxDepth levels (default 10 on the server,
    /// max 20), detecting circular ownership and identifying key persons.
    /// </summary>
    public Task<OwnershipResponse> TraceAsync(
        string uid,
        OwnershipRequest? request = null,
        CancellationToken ct = default)
        => _client.RequestAsync<OwnershipResponse>(
            HttpMethod.Post,
            $"/v1/ownership/{Uri.EscapeDataString(uid)}",
            request ?? new OwnershipRequest(),
            ct);
}
