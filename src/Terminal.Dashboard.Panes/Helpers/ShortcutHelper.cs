using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Terminal.Dashboard.Panes.Helpers;

public static class ShortcutHelper
{
    private const string ApiEndpoint = "https://api.app.shortcut.com";

    private static string ApiToken =>
        Environment.GetEnvironmentVariable("SHORTCUT_API_TOKEN") ?? string.Empty;

    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<List<ShortcutMember>> GetMembers()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiEndpoint}/api/v3/members");
        request.Headers.Add("Shortcut-Token", ApiToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var members = await response.Content.ReadFromJsonAsync<List<ShortcutMember>>();
        return members ?? new List<ShortcutMember>();
    }

    public static async Task<List<ShortcutWorkflow>> GetWorkflows()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiEndpoint}/api/v3/workflows");
        request.Headers.Add("Shortcut-Token", ApiToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var workflows = await response.Content.ReadFromJsonAsync<List<ShortcutWorkflow>>();
        return workflows ?? new List<ShortcutWorkflow>();
    }

    public static async Task<List<ShortcutStory>> SearchStories(string query, int pageSize = 25)
    {
        var (members, workflows) = await FetchMembersAndWorkflows();

        var stories = new List<ShortcutStory>();

        var url = $"{ApiEndpoint}/api/v3/search/stories?page_size={pageSize}&query={Uri.EscapeDataString(query)}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Shortcut-Token", ApiToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var searchResult = await response.Content.ReadFromJsonAsync<ShortcutSearchResult>();

        if (searchResult?.Data != null)
        {
            stories.AddRange(searchResult.Data);
        }

        var nextUrl = searchResult?.Next;
        while (!string.IsNullOrEmpty(nextUrl))
        {
            var nextRequest = new HttpRequestMessage(HttpMethod.Get, $"{ApiEndpoint}{nextUrl}");
            nextRequest.Headers.Add("Shortcut-Token", ApiToken);

            var nextResponse = await _httpClient.SendAsync(nextRequest);
            nextResponse.EnsureSuccessStatusCode();

            var nextResult = await nextResponse.Content.ReadFromJsonAsync<ShortcutSearchResult>();

            if (nextResult?.Data != null)
            {
                stories.AddRange(nextResult.Data);
            }

            nextUrl = nextResult?.Next;
        }

        // Enrich stories with member and workflow data
        foreach (var story in stories)
        {
            story.Owners = story.OwnerIds
                .Select(id => members.FirstOrDefault(m => m.Id == id))
                .Where(m => m != null)
                .Select(m => m!)
                .ToList();

            if (story.WorkflowId.HasValue && story.WorkflowStateId.HasValue)
            {
                var workflow = workflows.FirstOrDefault(w => w.Id == story.WorkflowId.Value);
                story.Workflow = workflow;

                if (workflow != null)
                {
                    story.WorkflowState = workflow.States.FirstOrDefault(s => s.Id == story.WorkflowStateId.Value);
                }
            }
        }

        return stories;
    }

    private static async Task<(List<ShortcutMember> Members, List<ShortcutWorkflow> Workflows)> FetchMembersAndWorkflows()
    {
        var membersTask = GetMembers();
        var workflowsTask = GetWorkflows();
        await Task.WhenAll(membersTask, workflowsTask);
        return (membersTask.Result, workflowsTask.Result);
    }

    public static class Formatter
    {
        private const int IdColumnWidth = 7;

        public static string Story(ShortcutStory story)
        {
            var id = story.Id;
            var name = story.Name;
            var state = story.WorkflowState?.Name ?? "Unknown";
            var owners = string.Join(", ", story.Owners.Select(m => m.Profile.MentionName));

            var idFormatted = $"sc-{id}";

            return $"[yellow]{idFormatted,-IdColumnWidth}[/] [cyan]{state}[/] [white]{owners}[/] [grey]{name}[/]";
        }
    }
}

public class ShortcutMember
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("profile")]
    public ShortcutMemberProfile Profile { get; set; } = new();
}

public class ShortcutMemberProfile
{
    [JsonPropertyName("mention_name")]
    public string MentionName { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class ShortcutWorkflow
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("states")]
    public List<ShortcutWorkflowState> States { get; set; } = new();
}

public class ShortcutWorkflowState
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class ShortcutStory
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("app_url")]
    public string AppUrl { get; set; } = string.Empty;

    [JsonPropertyName("story_type")]
    public string StoryType { get; set; } = string.Empty;

    [JsonPropertyName("workflow_id")]
    public long? WorkflowId { get; set; }

    [JsonPropertyName("workflow_state_id")]
    public long? WorkflowStateId { get; set; }

    [JsonPropertyName("owner_ids")]
    public List<string> OwnerIds { get; set; } = new();

    // Enriched fields (not from JSON)
    [JsonIgnore]
    public List<ShortcutMember> Owners { get; set; } = new();

    [JsonIgnore]
    public ShortcutWorkflow? Workflow { get; set; }

    [JsonIgnore]
    public ShortcutWorkflowState? WorkflowState { get; set; }
}

public class ShortcutSearchResult
{
    [JsonPropertyName("data")]
    public List<ShortcutStory> Data { get; set; } = new();

    [JsonPropertyName("next")]
    public string? Next { get; set; }
}
