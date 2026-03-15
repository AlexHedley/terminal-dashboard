using System.Text.Json;
using System.Text.Json.Serialization;

namespace Terminal.Dashboard.Panes.Helpers;

public static class CircleCIHelper
{
    private static readonly HttpClient _client = new HttpClient();
    private const string BaseUrl = "https://circleci.com/api/v2";

    public static async Task<List<CircleCIWorkflow>> GetWorkflows(string vcs, string org, string repo, int limitDays = 14)
    {
        var token = Environment.GetEnvironmentVariable("CIRCLECI_API_TOKEN");
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("CIRCLECI_API_TOKEN environment variable is not set.");

        var cutoffDate = DateTime.UtcNow.AddDays(-limitDays);

        using var pipelineRequest = new HttpRequestMessage(HttpMethod.Get,
            $"{BaseUrl}/project/{vcs}/{org}/{repo}/pipeline");
        pipelineRequest.Headers.Add("Circle-Token", token);

        var pipelineResponse = await _client.SendAsync(pipelineRequest);
        pipelineResponse.EnsureSuccessStatusCode();

        var pipelineContent = await pipelineResponse.Content.ReadAsStringAsync();
        var pipelineResult = JsonSerializer.Deserialize<CircleCIPipelineResponse>(pipelineContent);

        var recentPipelines = (pipelineResult?.Items ?? [])
            .Where(p => p.UpdatedAt >= cutoffDate)
            .ToList();

        var workflowTasks = recentPipelines.Select(pipeline => FetchFirstWorkflow(pipeline, token));
        var workflowResults = await Task.WhenAll(workflowTasks);

        return workflowResults.OfType<CircleCIWorkflow>().ToList();
    }

    private static async Task<CircleCIWorkflow?> FetchFirstWorkflow(CircleCIPipeline pipeline, string token)
    {
        using var workflowRequest = new HttpRequestMessage(HttpMethod.Get,
            $"{BaseUrl}/pipeline/{pipeline.Id}/workflow");
        workflowRequest.Headers.Add("Circle-Token", token);

        var workflowResponse = await _client.SendAsync(workflowRequest);
        if (!workflowResponse.IsSuccessStatusCode) return null;

        var workflowContent = await workflowResponse.Content.ReadAsStringAsync();
        var workflowResult = JsonSerializer.Deserialize<CircleCIWorkflowResponse>(workflowContent);

        var workflow = workflowResult?.Items?.FirstOrDefault();
        if (workflow != null)
            workflow.Pipeline = pipeline;

        return workflow;
    }
}

public class CircleCIPipelineResponse
{
    [JsonPropertyName("items")]
    public List<CircleCIPipeline>? Items { get; set; }
}

public class CircleCIPipeline
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("trigger")]
    public CircleCITrigger? Trigger { get; set; }

    [JsonPropertyName("vcs")]
    public CircleCIVcs? Vcs { get; set; }
}

public class CircleCITrigger
{
    [JsonPropertyName("actor")]
    public CircleCIActor? Actor { get; set; }
}

public class CircleCIActor
{
    [JsonPropertyName("login")]
    public string? Login { get; set; }
}

public class CircleCIVcs
{
    [JsonPropertyName("commit")]
    public CircleCICommit? Commit { get; set; }
}

public class CircleCICommit
{
    [JsonPropertyName("subject")]
    public string? Subject { get; set; }
}

public class CircleCIWorkflowResponse
{
    [JsonPropertyName("items")]
    public List<CircleCIWorkflow>? Items { get; set; }
}

public class CircleCIWorkflow
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("pipeline_number")]
    public int PipelineNumber { get; set; }

    [JsonPropertyName("project_slug")]
    public string ProjectSlug { get; set; } = string.Empty;

    // Not from JSON - set manually after fetching the workflow
    [JsonIgnore]
    public CircleCIPipeline? Pipeline { get; set; }
}
