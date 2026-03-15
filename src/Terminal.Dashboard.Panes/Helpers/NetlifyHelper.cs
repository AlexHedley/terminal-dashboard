using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Terminal.Dashboard.Panes.Helpers;

public static class NetlifyHelper
{
    private static readonly HttpClient _client = new HttpClient();

    public static async Task<List<NetlifyDeploy>> GetDeploys(string siteId)
    {
        var token = Environment.GetEnvironmentVariable("NETLIFY_TOKEN");

        var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://api.netlify.com/api/v1/sites/{siteId}/deploys");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        request.Headers.UserAgent.ParseAdd("Terminal.Dashboard");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var deploys = JsonSerializer.Deserialize<List<NetlifyDeploy>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return deploys ?? new List<NetlifyDeploy>();
    }
}

public class NetlifyDeploy
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("review_id")]
    public int? ReviewId { get; set; }

    [JsonPropertyName("context")]
    public string? Context { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("branch")]
    public string? Branch { get; set; }

    [JsonPropertyName("commit_ref")]
    public string? CommitRef { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("admin_url")]
    public string? AdminUrl { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("deploy_ssl_url")]
    public string? DeploySslUrl { get; set; }
}
