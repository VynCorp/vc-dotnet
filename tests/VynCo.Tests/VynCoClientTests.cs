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
            () => client.Companies.GetAsync("bad"));
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
            () => client.Teams.MembersAsync());
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
            resp.Headers.Add("X-RateLimit-Limit", "300");
            resp.Headers.Add("X-RateLimit-Remaining", "299");
            resp.Headers.Add("X-RateLimit-Reset", "1700000000");
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
        Assert.Equal(299, client.LastResponseHeaders.RateLimitRemaining);
        Assert.Equal(1700000000L, client.LastResponseHeaders.RateLimitReset);
        Assert.Contains("Zefix", client.LastResponseHeaders.DataSource);
    }

    // -- Companies resource tests --

    [Fact]
    public async Task Companies_Get_ReturnsCompany()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"uid":"CHE-123.456.789","name":"Test AG","canton":"ZH","auditorCategory":"Big4Competitor"}""");
        using var client = TestHelper.CreateClient(handler);

        var company = await client.Companies.GetAsync("CHE-123.456.789");

        Assert.Equal("CHE-123.456.789", company.Uid);
        Assert.Equal("Test AG", company.Name);
        Assert.Equal("ZH", company.Canton);
    }

    [Fact]
    public async Task Companies_Get_CorrectUrl()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"uid":"CHE-123.456.789","name":"Test AG"}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Companies.GetAsync("CHE-123.456.789");

        Assert.EndsWith("/v1/companies/CHE-123.456.789", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Companies_List_BuildsQueryString()
    {
        var json = """{"items":[],"total":0,"page":2,"pageSize":10}""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        await client.Companies.ListAsync(new CompanyListParams { Canton = "ZH", Page = 2, PageSize = 10 });

        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("page=2", uri);
        Assert.Contains("pageSize=10", uri);
        Assert.Contains("canton=ZH", uri);
    }

    [Fact]
    public async Task Companies_List_ParsesPagedResponse()
    {
        var json = """{"items":[{"uid":"CHE-111.222.333","name":"Foo GmbH"}],"total":1,"page":1,"pageSize":25}""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.ListAsync();

        Assert.Single(result.Items);
        Assert.Equal("CHE-111.222.333", result.Items[0].Uid);
        Assert.Equal(1, result.Total);
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
            """{"total":320000,"byStatus":{"active":250000},"byCanton":{"ZH":50000},"byLegalForm":{"AG":100000}}""");
        using var client = TestHelper.CreateClient(handler);

        var stats = await client.Companies.StatisticsAsync();

        Assert.Equal(320000, stats.Total);
        Assert.Equal(50000, stats.ByCanton["ZH"]);
    }

    [Fact]
    public async Task Companies_Compare_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"uids":["CHE-123.456.789"],"names":["Test AG"],"dimensions":[]}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.CompareAsync(
            new CompareRequest { Uids = new List<string> { "CHE-123.456.789", "CHE-987.654.321" } });

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("/v1/companies/compare", handler.LastRequest.RequestUri!.ToString());
        Assert.Contains("CHE-123.456.789", handler.LastRequestBody);
    }

    [Fact]
    public async Task Companies_Events_ReturnsEvents()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"events":[{"id":"ev-1","ceType":"company.changed","summary":"Name updated"}],"count":1}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.EventsAsync("CHE-123.456.789", limit: 5);

        Assert.Single(result.Events);
        Assert.Equal(1, result.Count);
        Assert.Contains("limit=5", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Companies_Relationships_ReturnsList()
    {
        var json = """[{"relatedUid":"CHE-111.222.333","relatedName":"Child GmbH","relationshipType":"Subsidiary","sharedPersons":["John Doe"]}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.RelationshipsAsync("CHE-123.456.789");

        Assert.Single(result);
        Assert.Equal("Subsidiary", result[0].RelationshipType);
    }

    [Fact]
    public async Task Companies_Fingerprint_ReturnsFingerprint()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"companyUid":"CHE-123.456.789","name":"Test AG","canton":"ZH","legalForm":"AG","boardSize":5,"companyAge":20,"hasParentCompany":false,"subsidiaryCount":3,"changeFrequency":12,"generatedAt":"2024-01-01","fingerprintVersion":"1.0"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.FingerprintAsync("CHE-123.456.789");

        Assert.Equal("CHE-123.456.789", result.CompanyUid);
        Assert.Equal(5, result.BoardSize);
        Assert.False(result.HasParentCompany);
    }

    [Fact]
    public async Task Companies_Nearby_BuildsQuery()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """[{"uid":"CHE-111.222.333","name":"Near AG","distance":0.5,"latitude":47.3769,"longitude":8.5417}]""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Companies.NearbyAsync(new NearbyParams { Lat = 47.3769, Lng = 8.5417, RadiusKm = 1.0 });

        Assert.Single(result);
        Assert.Contains("lat=47.3769", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- Changes resource tests --

    [Fact]
    public async Task Changes_ByCompany_ReturnsChanges()
    {
        var json = """[{"id":"ch-1","companyUid":"CHE-123.456.789","companyName":"Test AG","changeType":"AuditorChange","detectedAt":"2024-06-01"}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var changes = await client.Changes.ByCompanyAsync("CHE-123.456.789");

        Assert.Single(changes);
        Assert.Equal("AuditorChange", changes[0].ChangeType);
    }

    [Fact]
    public async Task Changes_List_BuildsQuery()
    {
        var json = """{"items":[],"total":0,"page":1,"pageSize":25}""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        await client.Changes.ListAsync(new ChangeListParams { ChangeType = "NameChange", Page = 1 });

        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("changeType=NameChange", uri);
        Assert.Contains("page=1", uri);
    }

    [Fact]
    public async Task Changes_Statistics_ReturnsStats()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"totalChanges":5000,"changesThisWeek":100,"changesThisMonth":400,"byType":{}}""");
        using var client = TestHelper.CreateClient(handler);

        var stats = await client.Changes.StatisticsAsync();

        Assert.Equal(5000, stats.TotalChanges);
        Assert.Equal(100, stats.ChangesThisWeek);
    }

    // -- Persons resource tests --

    [Fact]
    public async Task Persons_BoardMembers_ReturnsList()
    {
        var json = """[{"id":"p-1","firstName":"Hans","lastName":"Müller","role":"CEO","roleCategory":"Executive"}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var members = await client.Persons.BoardMembersAsync("CHE-123.456.789");

        Assert.Single(members);
        Assert.Equal("Hans", members[0].FirstName);
        Assert.Equal("CEO", members[0].Role);
        Assert.Contains("/v1/persons/board-members/CHE-123.456.789", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- Auditors resource tests --

    [Fact]
    public async Task Auditors_History_ReturnsHistory()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"companyUid":"CHE-123.456.789","companyName":"Test AG","currentAuditor":{"id":"t-1","auditorName":"PwC","isCurrent":true,"source":"Zefix"},"history":[]}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Auditors.HistoryAsync("CHE-123.456.789");

        Assert.Equal("CHE-123.456.789", result.CompanyUid);
        Assert.NotNull(result.CurrentAuditor);
        Assert.Equal("PwC", result.CurrentAuditor!.AuditorName);
    }

    [Fact]
    public async Task Auditors_Tenures_BuildsQuery()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"items":[],"total":0,"page":1,"pageSize":25}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Auditors.TenuresAsync(new AuditorTenureParams { MinYears = 10, Canton = "ZH" });

        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("minYears=10", uri);
        Assert.Contains("canton=ZH", uri);
    }

    // -- Dashboard resource tests --

    [Fact]
    public async Task Dashboard_Get_ReturnsDashboard()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"generatedAt":"2024-01-01","data":{"totalCompanies":500000,"completenessPct":85.5},"pipelines":[],"auditorTenures":{"totalTenures":100000,"avgTenureYears":4.5,"maxTenureYears":30.0,"longTenures7plus":15000}}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Dashboard.GetAsync();

        Assert.Equal(500000, result.Data.TotalCompanies);
        Assert.Equal(85.5, result.Data.CompletenessPct);
        Assert.Equal(100000, result.AuditorTenures.TotalTenures);
    }

    // -- Screening resource tests --

    [Fact]
    public async Task Screening_Screen_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"queryName":"Test Corp","screenedAt":"2024-01-01","hitCount":2,"riskLevel":"medium","hits":[],"sourcesChecked":["SECO","EU"]}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Screening.ScreenAsync(new ScreeningRequest { Name = "Test Corp" });

        Assert.Equal("medium", result.RiskLevel);
        Assert.Equal(2, result.HitCount);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("/v1/screening", handler.LastRequest.RequestUri!.ToString());
    }

    // -- Watchlists resource tests --

    [Fact]
    public async Task Watchlists_List_ReturnsList()
    {
        var json = """[{"id":"wl-1","name":"My List","description":"Test","companyCount":5,"createdAt":"2024-01-01"}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var lists = await client.Watchlists.ListAsync();

        Assert.Single(lists);
        Assert.Equal("My List", lists[0].Name);
        Assert.Equal(5, lists[0].CompanyCount);
    }

    [Fact]
    public async Task Watchlists_Create_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"id":"wl-1","name":"New List","description":"desc","createdAt":"2024-01-01","updatedAt":"2024-01-01"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Watchlists.CreateAsync(new CreateWatchlistRequest { Name = "New List", Description = "desc" });

        Assert.Equal("New List", result.Name);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }

    [Fact]
    public async Task Watchlists_AddCompanies_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"added":3}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Watchlists.AddCompaniesAsync("wl-1",
            new AddCompaniesRequest { Uids = new List<string> { "CHE-1", "CHE-2", "CHE-3" } });

        Assert.Equal(3, result.Added);
    }

    // -- Webhooks resource tests --

    [Fact]
    public async Task Webhooks_Create_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"webhook":{"id":"wh-1","url":"https://example.com/hook","description":"","eventFilters":[],"companyFilters":[],"status":"active","createdAt":"2024-01-01","updatedAt":"2024-01-01"},"signingSecret":"whsec_abc123"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Webhooks.CreateAsync(
            new CreateWebhookRequest { Url = "https://example.com/hook" });

        Assert.Equal("whsec_abc123", result.SigningSecret);
        Assert.Equal("wh-1", result.Webhook.Id);
    }

    [Fact]
    public async Task Webhooks_Test_SendsPost()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"success":true,"httpStatus":200}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Webhooks.TestAsync("wh-1");

        Assert.True(result.Success);
        Assert.Equal(200, result.HttpStatus);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }

    // -- Exports resource tests --

    [Fact]
    public async Task Exports_Create_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"id":"exp-1","status":"processing","format":"csv","createdAt":"2024-01-01"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Exports.CreateAsync(
            new CreateExportRequest { Format = "csv", Canton = "ZH" });

        Assert.Equal("exp-1", result.Id);
        Assert.Equal("processing", result.Status);
    }

    // -- AI resource tests --

    [Fact]
    public async Task Ai_Dossier_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"uid":"CHE-123.456.789","companyName":"Test AG","dossier":"A detailed analysis...","sources":["Zefix"],"generatedAt":"2024-01-01"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Ai.DossierAsync(new AiDossierRequest { Uid = "CHE-123.456.789" });

        Assert.Equal("Test AG", result.CompanyName);
        Assert.Equal("A detailed analysis...", result.Dossier);
        Assert.Contains("/v1/ai/dossier", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Ai_RiskScore_ReturnsScore()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"uid":"CHE-123.456.789","companyName":"Test AG","overallScore":35,"riskLevel":"low","breakdown":[],"assessedAt":"2024-01-01"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Ai.RiskScoreAsync(new RiskScoreRequest { Uid = "CHE-123.456.789" });

        Assert.Equal(35, result.OverallScore);
        Assert.Equal("low", result.RiskLevel);
    }

    [Fact]
    public async Task Ai_Search_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"query":"fintech in Zurich","explanation":"Searching for...","filtersApplied":{},"results":[],"total":0}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Ai.SearchAsync(new AiSearchRequest { Query = "fintech in Zurich" });

        Assert.Equal("fintech in Zurich", result.Query);
        Assert.Contains("/v1/ai/search", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- Graph resource tests --

    [Fact]
    public async Task Graph_Get_ReturnsGraph()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"nodes":[{"id":"n-1","name":"Test AG","uid":"CHE-123.456.789","type":"company"}],"links":[]}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Graph.GetAsync("CHE-123.456.789");

        Assert.Single(result.Nodes);
        Assert.Equal("company", result.Nodes[0].NodeType);
        Assert.Contains("/v1/graph/CHE-123.456.789", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Graph_Analyze_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"nodes":[],"links":[],"clusters":[{"id":0,"companyUids":["CHE-1"],"sharedPersons":["John"]}]}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Graph.AnalyzeAsync(
            new NetworkAnalysisRequest { Uids = new List<string> { "CHE-1", "CHE-2" }, Overlay = "auditor" });

        Assert.Single(result.Clusters);
        Assert.Contains("/v1/network/analyze", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- Dossiers resource tests --

    [Fact]
    public async Task Dossiers_Create_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"id":"d-1","userId":"u-1","companyUid":"CHE-123.456.789","companyName":"Test AG","level":"standard","content":"Report...","sources":[],"createdAt":"2024-01-01"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Dossiers.CreateAsync(
            new CreateDossierRequest { Uid = "CHE-123.456.789", Level = "standard" });

        Assert.Equal("d-1", result.Id);
        Assert.Equal("Report...", result.Content);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }

    [Fact]
    public async Task Dossiers_List_ReturnsSummaries()
    {
        var json = """[{"id":"d-1","companyUid":"CHE-123.456.789","companyName":"Test AG","level":"standard","createdAt":"2024-01-01"}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Dossiers.ListAsync();

        Assert.Single(result);
        Assert.Equal("d-1", result[0].Id);
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
        Assert.Equal(0.01, balance.OverageRate);
    }

    [Fact]
    public async Task Credits_Usage_ReturnsUsage()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"operations":[{"operation":"companies.get","count":100,"totalCredits":100}],"total":100,"period":{"since":"2024-01-01","until":"2024-02-01"}}""");
        using var client = TestHelper.CreateClient(handler);

        var usage = await client.Credits.UsageAsync(since: "2024-01-01");

        Assert.Single(usage.Operations);
        Assert.Equal(100, usage.Total);
        Assert.Contains("since=2024-01-01", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Credits_History_ReturnsHistory()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"items":[{"id":1,"entryType":"debit","amount":-1,"balance":49999,"description":"companies.get","createdAt":"2024-01-01"}],"total":1}""");
        using var client = TestHelper.CreateClient(handler);

        var history = await client.Credits.HistoryAsync(limit: 10, offset: 0);

        Assert.Single(history.Items);
        Assert.Equal(1, history.Total);
        Assert.Contains("limit=10", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- API Keys resource tests --

    [Fact]
    public async Task ApiKeys_Create_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"key":"vc_live_abc123","id":"k-1","name":"My Key","prefix":"vc_live_","environment":"live","scopes":[],"createdAt":"2024-01-01","warning":"Store this key securely"}""");
        using var client = TestHelper.CreateClient(handler);

        var created = await client.ApiKeys.CreateAsync(
            new CreateApiKeyRequest { Name = "My Key", Environment = "live" });

        Assert.Equal("vc_live_abc123", created.Key);
        Assert.Equal("My Key", created.Name);
        Assert.Contains("\"name\":\"My Key\"", handler.LastRequestBody);
    }

    // -- Teams resource tests --

    [Fact]
    public async Task Teams_Me_ReturnsTeam()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"id":"t-1","name":"My Team","slug":"my-team","tier":"professional","creditBalance":250000,"monthlyCredits":250000}""");
        using var client = TestHelper.CreateClient(handler);

        var team = await client.Teams.MeAsync();

        Assert.Equal("My Team", team.Name);
        Assert.Equal("professional", team.Tier);
        Assert.Equal(250000, team.CreditBalance);
    }

    [Fact]
    public async Task Teams_Members_ReturnsList()
    {
        var json = """[{"id":"m-1","name":"John Doe","email":"john@example.com","role":"Admin"}]""";
        var handler = new MockHttpHandler(HttpStatusCode.OK, json);
        using var client = TestHelper.CreateClient(handler);

        var members = await client.Teams.MembersAsync();

        Assert.Single(members);
        Assert.Equal("John Doe", members[0].Name);
        Assert.Contains("/v1/teams/me/members", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Teams_InviteMember_ReturnsInvitation()
    {
        var handler = new MockHttpHandler(HttpStatusCode.Created,
            """{"id":"inv-1","teamId":"t-1","email":"jane@example.com","role":"Member","token":"tok_abc","status":"pending","createdAt":"2024-01-01","expiresAt":"2024-01-08"}""");
        using var client = TestHelper.CreateClient(handler);

        var invitation = await client.Teams.InviteMemberAsync(
            new InviteMemberRequest { Email = "jane@example.com", Role = "Member" });

        Assert.Equal("jane@example.com", invitation.Email);
        Assert.Equal("pending", invitation.Status);
        Assert.Contains("jane@example.com", handler.LastRequestBody);
    }

    [Fact]
    public async Task Teams_BillingSummary_ReturnsSummary()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"tier":"professional","creditBalance":250000,"monthlyCredits":250000,"usedThisMonth":500,"members":[{"userId":"u-1","name":"John","creditsUsed":500}]}""");
        using var client = TestHelper.CreateClient(handler);

        var summary = await client.Teams.BillingSummaryAsync();

        Assert.Equal(500, summary.UsedThisMonth);
        Assert.Single(summary.Members);
        Assert.Contains("/v1/teams/me/billing-summary", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- Analytics resource tests --

    [Fact]
    public async Task Analytics_Cluster_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"clusters":[]}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Analytics.ClusterAsync(new ClusterRequest { Algorithm = "kmeans", K = 3 });

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("/v1/analytics/cluster", handler.LastRequest.RequestUri!.ToString());
        Assert.Contains("\"algorithm\":\"kmeans\"", handler.LastRequestBody);
    }

    [Fact]
    public async Task Analytics_Cantons_ReturnsList()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """[{"canton":"ZH","count":50000,"percentage":15.5}]""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Analytics.CantonsAsync();

        Assert.Single(result);
        Assert.Equal("ZH", result[0].Canton);
        Assert.Equal(15.5, result[0].Percentage);
    }

    [Fact]
    public async Task Analytics_Candidates_BuildsQuery()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"items":[],"total":0,"page":1,"pageSize":25}""");
        using var client = TestHelper.CreateClient(handler);

        await client.Analytics.CandidatesAsync(new CandidateParams { Canton = "BE", SortBy = "shareCapital" });

        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("canton=BE", uri);
        Assert.Contains("sortBy=shareCapital", uri);
    }

    // -- Health resource tests --

    [Fact]
    public async Task Health_Check_ReturnsStatus()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK,
            """{"status":"healthy","database":"ok","redis":"ok","version":"2.0.0"}""");
        using var client = TestHelper.CreateClient(handler);

        var health = await client.Health.CheckAsync();

        Assert.Equal("healthy", health.Status);
        Assert.Equal("ok", health.Database);
        Assert.EndsWith("/health", handler.LastRequest!.RequestUri!.ToString());
    }

    // -- Billing resource tests --

    [Fact]
    public async Task Billing_CreateCheckout_SendsRequest()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"url":"https://checkout.stripe.com/session"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Billing.CreateCheckoutAsync(new CheckoutRequest { Tier = "professional" });

        Assert.Equal("https://checkout.stripe.com/session", result.Url);
        Assert.Contains("/v1/billing/checkout-session", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Billing_CreatePortal_SendsPost()
    {
        var handler = new MockHttpHandler(HttpStatusCode.OK, """{"url":"https://billing.stripe.com/portal"}""");
        using var client = TestHelper.CreateClient(handler);

        var result = await client.Billing.CreatePortalAsync();

        Assert.Equal("https://billing.stripe.com/portal", result.Url);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }

    // -- Dispose tests --

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var client = new VynCoClient("vc_test_key");
        client.Dispose();
        client.Dispose(); // should not throw
    }

    [Fact]
    public void SdkVersion_Is2()
    {
        Assert.Equal("2.0.0", VynCoClient.SdkVersion);
    }
}
