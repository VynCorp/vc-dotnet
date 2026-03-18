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

        Assert.Contains($"vynco-dotnet/{VynCoClient.SdkVersion}", handler.LastRequest!.Headers.UserAgent.ToString());
    }

    // -- Exception mapping tests --

    [Fact]
    public async Task Request_400_ThrowsBadRequestException()
    {
        var handler = new MockHttpHandler(HttpStatusCode.BadRequest,
            """{"type":"about:blank","title":"Bad Request","status":400,"detail":"Invalid query parameter"}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => client.Companies.SearchAsync(new CompanySearchRequest { Query = "a" }));
        Assert.Equal(400, ex.StatusCode);
    }

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
    public async Task Request_403_ThrowsForbiddenException()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Forbidden,
            """{"type":"about:blank","title":"Forbidden","status":403,"detail":"Insufficient permissions"}""");
        using var client = TestHelper.CreateClient(handler);

        var ex = await Assert.ThrowsAsync<ForbiddenException>(
            () => client.Teams.ListMembersAsync());
        Assert.Equal(403, ex.StatusCode);
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

    // -- Response headers tests --

    [Fact]
    public async Task Request_CapturesResponseHeaders()
    {
        var handler = new MockHttpHandler(req =>
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"uid":"CHE-123.456.789","name":"Test AG"}""",
                    System.Text.Encoding.UTF8, "application/json")
            };
            resp.Headers.Add("X-Request-Id", "req-abc-123");
            resp.Headers.Add("X-Credits-Used", "1");
            resp.Headers.Add("X-Credits-Remaining", "49999");
            resp.Headers.Add("X-Rate-Limit-Limit", "300");
            resp.Headers.Add("X-Data-Source", "Zefix / Federal Commercial Registry Office (EHRA)");
            return resp;
        });
        using var client = TestHelper.CreateClient(handler);

        await client.Companies.GetAsync("CHE-123.456.789");

        Assert.NotNull(client.LastResponseHeaders);
        Assert.Equal("req-abc-123", client.LastResponseHeaders!.RequestId);
        Assert.Equal(1, client.LastResponseHeaders.CreditsUsed);
        Assert.Equal(49999, client.LastResponseHeaders.CreditsRemaining);
        Assert.Equal(300, client.LastResponseHeaders.RateLimitLimit);
        Assert.Contains("Zefix", client.LastResponseHeaders.DataSource);
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
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"uid":"CHE-123.456.789","name":"Test AG","createdAt":"2024-01-01T00:00:00Z","updatedAt":"2024-01-01T00:00:00Z"}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Companies.GetAsync("CHE-123.456.789");

        Assert.EndsWith("/api/v1/companies/CHE-123.456.789", handler.LastRequest!.RequestUri!.ToString());
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
        var json = """{"items":[{"uid":"CHE-111.222.333","name":"Foo GmbH","isActive":true,"createdAt":"2024-01-01T00:00:00Z","updatedAt":"2024-01-01T00:00:00Z"}],"totalCount":1,"page":1,"pageSize":25,"totalPages":1,"hasPreviousPage":false,"hasNextPage":false}""";
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
        var json = """[{"uid":"CHE-111.222.333","name":"Acme AG","isActive":true,"createdAt":"2024-01-01T00:00:00Z","updatedAt":"2024-01-01T00:00:00Z"}]""";
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

    [Fact]
    public async Task Companies_Compare_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"companies":[]}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.CompareAsync(
            new CompanyCompareRequest { Uids = new List<string> { "CHE-123.456.789", "CHE-987.654.321" } });

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("/api/v1/companies/compare", handler.LastRequest.RequestUri!.ToString());
        Assert.Contains("CHE-123.456.789", handler.LastRequestBody);
    }

    [Fact]
    public async Task Companies_GetRelationships_ReturnsRelationships()
    {
        var json = """{"companyUid":"CHE-123.456.789","total":2,"relationships":[{"id":"00000000-0000-0000-0000-000000000001","sourceCompanyUid":"CHE-123.456.789","sourceCompanyName":"Parent AG","targetCompanyUid":"CHE-111.222.333","targetCompanyName":"Child GmbH","relationshipType":"Subsidiary","dataSource":"Zefix","isActive":true}]}""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.GetRelationshipsAsync("CHE-123.456.789");

        Assert.Equal("CHE-123.456.789", result.CompanyUid);
        Assert.Single(result.Relationships);
        Assert.Equal("Subsidiary", result.Relationships[0].RelationshipType);
    }

    // -- Changes resource tests --

    [Fact]
    public async Task Changes_GetByCompany_ReturnsChanges()
    {
        var json = """[{"id":"00000000-0000-0000-0000-000000000001","companyUid":"CHE-123.456.789","companyName":"Test AG","changeType":"AuditorChange","detectedAt":"2024-06-01T00:00:00Z","isReviewed":false,"isFlagged":false}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var changes = await client.Changes.GetByCompanyAsync("CHE-123.456.789");

        Assert.Single(changes);
        Assert.Equal("AuditorChange", changes[0].ChangeType);
        Assert.Equal("Test AG", changes[0].CompanyName);
    }

    [Fact]
    public async Task Changes_GetBySogcId_ReturnsChanges()
    {
        var json = """[{"id":"00000000-0000-0000-0000-000000000001","companyUid":"CHE-123.456.789","changeType":"NameChange","sogcId":"HR02-1234567","detectedAt":"2024-06-01T00:00:00Z","isReviewed":false,"isFlagged":false}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var changes = await client.Changes.GetBySogcIdAsync("HR02-1234567");

        Assert.Single(changes);
        Assert.Contains("/api/v1/changes/sogc/HR02-1234567", handler.LastRequest!.RequestUri!.ToString());
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
            """{"id":"00000000-0000-0000-0000-000000000001","name":"My Key","prefix":"vc_live_","isTestKey":false,"isActive":true,"createdAt":"2024-01-01T00:00:00Z","key":"vc_live_abc123def456"}""");
        using var client = TestHelper.CreateClient(handler);

        var created = await client.ApiKeys.CreateAsync(new CreateApiKeyRequest { Name = "My Key", IsTestKey = false });

        Assert.Equal("vc_live_abc123def456", created.Key);
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

    [Fact]
    public async Task Teams_ListMembers_ReturnsList()
    {
        var json = """[{"id":"00000000-0000-0000-0000-000000000001","name":"John Doe","email":"john@example.com","role":"Admin","isActive":true}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var members = await client.Teams.ListMembersAsync();

        Assert.Single(members);
        Assert.Equal("John Doe", members[0].Name);
        Assert.Equal("Admin", members[0].Role);
        Assert.Contains("/api/v1/teams/me/members", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Teams_InviteMember_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"id":"00000000-0000-0000-0000-000000000002","name":"Jane","email":"jane@example.com","role":"Member","isActive":false}""");
        using var client = TestHelper.CreateClient(handler);

        var member = await client.Teams.InviteMemberAsync(
            new InviteMemberRequest { Email = "jane@example.com", Name = "Jane", Role = "Member" });

        Assert.Equal("jane@example.com", member.Email);
        Assert.Contains("jane@example.com", handler.LastRequestBody);
    }

    [Fact]
    public async Task Teams_BillingSummary_ReturnsSummary()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"members":[{"memberName":"John","creditsUsed":500,"percentage":100.0}],"totalCreditsUsed":500,"period":"2024-06"}""");
        using var client = TestHelper.CreateClient(handler);

        var summary = await client.Teams.BillingSummaryAsync();

        Assert.Equal(500, summary.TotalCreditsUsed);
        Assert.Single(summary.Members);
        Assert.Contains("/api/v1/teams/me/billing-summary", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- Watches resource tests --

    [Fact]
    public async Task Watches_List_ReturnsList()
    {
        var json = """[{"id":"00000000-0000-0000-0000-000000000001","companyUid":"CHE-123.456.789","companyName":"Test AG","channel":"InApp","watchedChangeTypes":["NameChange"],"createdAt":"2024-01-01T00:00:00Z"}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var watches = await client.Watches.ListAsync();

        Assert.Single(watches);
        Assert.Equal("CHE-123.456.789", watches[0].CompanyUid);
        Assert.Contains("/api/v1/watches", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Watches_Add_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"id":"00000000-0000-0000-0000-000000000001","companyUid":"CHE-123.456.789","companyName":"Test AG","channel":"InApp","watchedChangeTypes":[],"createdAt":"2024-01-01T00:00:00Z"}""");
        using var client = TestHelper.CreateClient(handler);

        var watch = await client.Watches.AddAsync(new AddWatchRequest { CompanyUid = "CHE-123.456.789" });

        Assert.Equal("CHE-123.456.789", watch.CompanyUid);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }

    [Fact]
    public async Task Watches_ListNotifications_ReturnsList()
    {
        var json = """[{"id":"00000000-0000-0000-0000-000000000001","companyUid":"CHE-123.456.789","companyName":"Test AG","changeId":"00000000-0000-0000-0000-000000000002","changeType":"NameChange","summary":"Name changed","channel":"InApp","status":"sent","createdAt":"2024-01-01T00:00:00Z"}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var notifications = await client.Watches.ListNotificationsAsync();

        Assert.Single(notifications);
        Assert.Equal("NameChange", notifications[0].ChangeType);
    }

    // -- Analytics resource tests --

    [Fact]
    public async Task Analytics_Cluster_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"clusters":[],"inertia":0.0}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Analytics.ClusterAsync(new ClusteringRequest { K = 3, Canton = "ZH" });

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("/api/v1/analytics/cluster", handler.LastRequest.RequestUri!.ToString());
        Assert.Contains("\"k\":3", handler.LastRequestBody);
    }

    [Fact]
    public async Task Analytics_Velocity_BuildsQuery()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"data":[]}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Analytics.VelocityAsync(new VelocityParams { Days = 7 });

        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("/api/v1/analytics/velocity", uri);
        Assert.Contains("days=7", uri);
    }

    // -- Health resource tests --

    [Fact]
    public async Task Health_Check_ReturnsStatus()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"status":"Healthy","uptime":"3d 12h","checks":[{"name":"Database","status":"Healthy","durationMs":5}]}""");
        using var client = TestHelper.CreateClient(handler);

        var health = await client.Health.CheckAsync();

        Assert.Equal("Healthy", health.Status);
        Assert.Single(health.Checks);
        Assert.Equal("Database", health.Checks[0].Name);
    }

    // -- News resource tests --

    [Fact]
    public async Task News_GetRecent_ReturnsNews()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"count":1,"items":[{"title":"Breaking news"}]}""");
        using var client = TestHelper.CreateClient(handler);

        var news = await client.News.GetRecentAsync(limit: 10);

        Assert.Equal(1, news.Count);
        Assert.Contains("/api/v1/news/recent?limit=10", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- Dossiers resource tests --

    [Fact]
    public async Task Dossiers_Generate_ReturnsDossier()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"companyUid":"CHE-123.456.789","companyName":"Test AG","summary":"A test company","riskScore":0.3,"generatedAt":"2024-06-01T00:00:00Z","status":"completed"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Dossiers.GenerateAsync("CHE-123.456.789",
            new GenerateDossierRequest { Type = "comprehensive" });

        Assert.Equal("completed", result.Status);
        Assert.Equal("Test AG", result.CompanyName);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("comprehensive", handler.LastRequestBody);
    }

    // -- Persons resource tests --

    [Fact]
    public async Task Persons_List_BuildsQuery()
    {
        var json = """[{"id":"00000000-0000-0000-0000-000000000001","name":"Hans Müller","roles":["CEO"],"companies":["Test AG"]}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var persons = await client.Persons.ListAsync(new ListPersonsParams { Search = "Müller", PageSize = 10 });

        Assert.Single(persons);
        Assert.Equal("Hans Müller", persons[0].Name);
        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("pageSize=10", uri);
        Assert.True(
            uri.Contains("search=M%C3%BCller") || uri.Contains("search=Müller"),
            $"Expected URI to contain encoded or literal umlaut, got: {uri}");
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

    // -- Dispose tests --

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var client = new VynCoClient("vc_test_key");
        client.Dispose();
        client.Dispose(); // should not throw
    }
}
