using System.Net;
using System.Text.Json;
using VynCo.Models;

namespace VynCo.Tests;

public class VynCoClientTests
{
    // -- Constructor tests --

    [Fact]
    public void Constructor_EmptyApiKey_Throws()
    {
        Assert.Throws<ArgumentException>(() => new VynCoClient(""));
    }

    [Fact]
    public void Constructor_WhitespaceApiKey_Throws()
    {
        Assert.Throws<ArgumentException>(() => new VynCoClient("   "));
    }

    // -- Auth header tests --

    [Fact]
    public async Task Request_SendsBearerToken()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"uid":"CHE-123.456.789","name":"Test AG"}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Companies.GetAsync("CHE-123.456.789");

        Assert.NotNull(handler.LastRequest);
        Assert.Equal("Bearer", handler.LastRequest!.Headers.Authorization?.Scheme);
        Assert.Equal("vc_test_key", handler.LastRequest.Headers.Authorization?.Parameter);
    }

    [Fact]
    public async Task Request_SetsUserAgent()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"uid":"CHE-123.456.789","name":"Test AG"}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Companies.GetAsync("CHE-123.456.789");

        Assert.Contains("vynco-dotnet/0.1.0", handler.LastRequest!.Headers.UserAgent.ToString());
    }

    // -- Exception mapping tests --

    [Fact]
    public async Task Request_401_ThrowsAuthenticationException()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Unauthorized,
            """{"type":"about:blank","title":"Unauthorized","status":401,"detail":"Invalid API key"}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<AuthenticationException>(
            () => client.Companies.GetAsync("CHE-123.456.789"));
        Assert.Equal(401, ex.StatusCode);
        Assert.Equal("Invalid API key", ex.Message);
    }

    [Fact]
    public async Task Request_402_ThrowsInsufficientCreditsException()
    {
        var handler = new MockHttpHandler(HttpStatusCode.PaymentRequired,
            """{"type":"about:blank","title":"Payment Required","status":402,"detail":"Insufficient credits"}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<InsufficientCreditsException>(
            () => client.Companies.GetAsync("CHE-123.456.789"));
        Assert.Equal(402, ex.StatusCode);
    }

    [Fact]
    public async Task Request_404_ThrowsNotFoundException()
    {
        var handler = new MockHttpHandler(HttpStatusCode.NotFound,
            """{"type":"about:blank","title":"Not Found","status":404,"detail":"Company not found"}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => client.Companies.GetAsync("CHE-000.000.000"));
        Assert.Equal(404, ex.StatusCode);
        Assert.Equal("Company not found", ex.Message);
    }

    [Fact]
    public async Task Request_422_ThrowsValidationException()
    {
        var handler = new MockHttpHandler((HttpStatusCode)422,
            """{"type":"about:blank","title":"Validation Error","status":422,"detail":"Invalid UID format"}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => client.Companies.GetAsync("INVALID"));
        Assert.Equal(422, ex.StatusCode);
    }

    [Fact]
    public async Task Request_429_ThrowsRateLimitException()
    {
        var handler = new MockHttpHandler((HttpStatusCode)429,
            """{"type":"about:blank","title":"Too Many Requests","status":429,"detail":"Rate limit exceeded"}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<RateLimitException>(
            () => client.Companies.GetAsync("CHE-123.456.789"));
        Assert.Equal(429, ex.StatusCode);
    }

    [Fact]
    public async Task Request_500_ThrowsServerException()
    {
        var handler = new MockHttpHandler(HttpStatusCode.InternalServerError,
            """{"type":"about:blank","title":"Internal Server Error","status":500}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<ServerException>(
            () => client.Companies.GetAsync("CHE-123.456.789"));
        Assert.Equal(500, ex.StatusCode);
    }

    [Fact]
    public async Task Request_ProblemDetailsBody_IsParsed()
    {
        var handler = new MockHttpHandler(HttpStatusCode.NotFound,
            """{"type":"https://api.vynco.ch/errors/not-found","title":"Not Found","status":404,"detail":"Company not found","traceId":"abc-123"}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => client.Companies.GetAsync("CHE-000.000.000"));
        Assert.NotNull(ex.Body);
        Assert.Equal("https://api.vynco.ch/errors/not-found", ex.Body!.Type);
        Assert.Equal("abc-123", ex.Body.TraceId);
    }

    // -- Companies resource tests --

    [Fact]
    public async Task Companies_Get_ReturnsCompany()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"uid":"CHE-123.456.789","name":"Test AG","canton":"ZH","isActive":true,"auditorCategory":"Big4Competitor","createdAt":"2024-01-01T00:00:00Z","updatedAt":"2024-06-01T00:00:00Z"}""");
        using var client = TestHelper.CreateClient(handler);

        var company = await client.Companies.GetAsync("CHE-123.456.789");

        Assert.Equal("CHE-123.456.789", company.Uid);
        Assert.Equal("Test AG", company.Name);
        Assert.Equal("ZH", company.Canton);
        Assert.True(company.IsActive);
    }

    [Fact]
    public async Task Companies_Get_CorrectUrl()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"uid":"CHE-123.456.789","name":"Test AG","auditorCategory":"","createdAt":"2024-01-01T00:00:00Z","updatedAt":"2024-01-01T00:00:00Z"}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Companies.GetAsync("CHE-123.456.789");

        Assert.EndsWith("/v1/companies/CHE-123.456.789", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Companies_List_BuildsQueryString()
    {
        var json = """{"items":[],"totalCount":0,"page":1,"pageSize":25,"totalPages":0,"hasPreviousPage":false,"hasNextPage":false}""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        await client.Companies.ListAsync(new ListCompaniesParams { Canton = "ZH", Page = 2, PageSize = 10 });

        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("page=2", uri);
        Assert.Contains("pageSize=10", uri);
        Assert.Contains("canton=ZH", uri);
    }

    [Fact]
    public async Task Companies_List_ParsesPagedResponse()
    {
        var json = """{"items":[{"uid":"CHE-111.222.333","name":"Foo GmbH","auditorCategory":"None","isActive":true,"createdAt":"2024-01-01T00:00:00Z","updatedAt":"2024-01-01T00:00:00Z"}],"totalCount":1,"page":1,"pageSize":25,"totalPages":1,"hasPreviousPage":false,"hasNextPage":false}""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.ListAsync();

        Assert.Single(result.Items);
        Assert.Equal("CHE-111.222.333", result.Items[0].Uid);
        Assert.Equal(1, result.TotalCount);
        Assert.False(result.HasNextPage);
    }

    [Fact]
    public async Task Companies_Search_SendsPostBody()
    {
        var json = """[{"uid":"CHE-111.222.333","name":"Acme AG","auditorCategory":"","isActive":true,"createdAt":"2024-01-01T00:00:00Z","updatedAt":"2024-01-01T00:00:00Z"}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var results = await client.Companies.SearchAsync(new CompanySearchRequest { Query = "Acme", Limit = 10 });

        Assert.Single(results);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("\"query\":\"Acme\"", handler.LastRequestBody);
    }

    [Fact]
    public async Task Companies_Count_ReturnsCount()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"count":320000}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.CountAsync();

        Assert.Equal(320000, result.Count);
    }

    [Fact]
    public async Task Companies_Statistics_ReturnsStats()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"totalCount":320000,"enrichedCount":100000,"cantonCounts":{"ZH":50000,"BE":30000},"auditorCategoryCounts":{"None":200000}}""");
        using var client = TestHelper.CreateClient(handler);

        var stats = await client.Companies.StatisticsAsync();

        Assert.Equal(320000, stats.TotalCount);
        Assert.Equal(50000, stats.CantonCounts["ZH"]);
    }

    // -- Changes resource tests --

    [Fact]
    public async Task Changes_GetByCompany_ReturnsChanges()
    {
        var json = """[{"id":"00000000-0000-0000-0000-000000000001","companyUid":"CHE-123.456.789","changeType":"AuditorChange","detectedAt":"2024-06-01T00:00:00Z","isReviewed":false,"isFlagged":false}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var changes = await client.Changes.GetByCompanyAsync("CHE-123.456.789");

        Assert.Single(changes);
        Assert.Equal("AuditorChange", changes[0].ChangeType);
    }

    // -- Credits resource tests --

    [Fact]
    public async Task Credits_Balance_ReturnsBalance()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"balance":49500,"monthlyCredits":50000,"usedThisMonth":500,"tier":"starter","overageRate":0.01}""");
        using var client = TestHelper.CreateClient(handler);

        var balance = await client.Credits.BalanceAsync();

        Assert.Equal(49500, balance.Balance);
        Assert.Equal("starter", balance.Tier);
        Assert.Equal(0.01m, balance.OverageRate);
    }

    // -- API Keys resource tests --

    [Fact]
    public async Task ApiKeys_Create_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"id":"00000000-0000-0000-0000-000000000001","name":"My Key","keyPrefix":"vc_live_","keyHint":"abc...xyz","permissions":["read"],"isActive":true,"createdAt":"2024-01-01T00:00:00Z","rawKey":"vc_live_abc123def456"}""");
        using var client = TestHelper.CreateClient(handler);

        var created = await client.ApiKeys.CreateAsync(new CreateApiKeyRequest { Name = "My Key", IsTestKey = false });

        Assert.Equal("vc_live_abc123def456", created.RawKey);
        Assert.Equal("My Key", created.Name);
        Assert.Contains("\"name\":\"My Key\"", handler.LastRequestBody);
    }

    // -- Teams resource tests --

    [Fact]
    public async Task Teams_GetCurrent_ReturnsTeam()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"id":"00000000-0000-0000-0000-000000000001","name":"My Team","slug":"my-team","tier":"professional","creditBalance":250000,"monthlyCredits":250000,"overageRate":0.005,"createdAt":"2024-01-01T00:00:00Z"}""");
        using var client = TestHelper.CreateClient(handler);

        var team = await client.Teams.GetCurrentAsync();

        Assert.Equal("My Team", team.Name);
        Assert.Equal("professional", team.Tier);
        Assert.Equal(250000, team.CreditBalance);
    }

    // -- Dispose tests --

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var client = new VynCoClient("vc_test_key");
        client.Dispose();
        client.Dispose(); // should not throw
    }

    // -- Dossiers resource tests --

    [Fact]
    public async Task Dossiers_Generate_ReturnsAccepted()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Accepted,
            """{"dossierId":"00000000-0000-0000-0000-000000000001","status":"pending"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Dossiers.GenerateAsync("CHE-123.456.789");

        Assert.Equal("pending", result.Status);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }

    // -- Persons resource tests --

    [Fact]
    public async Task Persons_Search_BuildsQuery()
    {
        var json = """[{"id":"00000000-0000-0000-0000-000000000001","fullName":"Hans Müller","roleCount":3,"activeRoleCount":2}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var persons = await client.Persons.SearchAsync(new SearchPersonsParams { Query = "Müller", Limit = 10 });

        Assert.Single(persons);
        Assert.Equal("Hans Müller", persons[0].FullName);
        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.True(
            uri.Contains("q=M%C3%BCller") || uri.Contains("q=Müller"),
            $"Expected URI to contain encoded or literal umlaut, got: {uri}");
    }

    // -- Webhooks resource tests --

    [Fact]
    public async Task Webhooks_Create_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"id":"wh_1","url":"https://example.com/hook","events":["company.changed"],"secret":"whsec_abc123"}""");
        using var client = TestHelper.CreateClient(handler);

        var webhook = await client.Webhooks.CreateAsync(new CreateWebhookRequest
        {
            Url = "https://example.com/hook",
            Events = new List<string> { "company.changed" }
        });

        Assert.Equal("whsec_abc123", webhook.Secret);
        Assert.Contains("company.changed", handler.LastRequestBody);
    }

    // -- SyncStatus resource tests --

    [Fact]
    public async Task SyncStatus_Get_ReturnsList()
    {
        var json = """[{"id":"zefix-sync","status":"completed","itemsProcessed":320000,"itemsTotal":320000}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var statuses = await client.SyncStatus.GetAsync();

        Assert.Single(statuses);
        Assert.Equal("completed", statuses[0].Status);
        Assert.Equal(320000, statuses[0].ItemsProcessed);
    }
}
