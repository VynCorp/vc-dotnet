using System.Reflection;

namespace VynCo.Tests;

public static class TestHelper
{
    public static VynCoClient CreateClient(HttpMessageHandler handler, string baseUrl = "http://localhost")
    {
        var client = new VynCoClient("vc_test_key", baseUrl: baseUrl, maxRetries: 0);

        // Replace the internal HttpClient via reflection
        var field = typeof(VynCoClient).GetField("_http", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var oldHttp = (HttpClient)field.GetValue(client)!;
        oldHttp.Dispose();

        var newHttp = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
        newHttp.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "vc_test_key");
        newHttp.DefaultRequestHeaders.UserAgent.ParseAdd($"vynco-dotnet/{VynCoClient.SdkVersion}");
        newHttp.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        field.SetValue(client, newHttp);

        return client;
    }
}
