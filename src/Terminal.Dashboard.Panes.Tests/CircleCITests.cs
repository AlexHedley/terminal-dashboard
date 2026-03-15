using System.Text.Json;
using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class CircleCITests
{
    [Test]
    public void CircleCIPipeline_Deserialize_SetsProperties()
    {
        var json = """
            {
                "id": "abc-123",
                "number": 42,
                "updated_at": "2024-01-15T10:00:00Z",
                "trigger": {
                    "actor": {
                        "login": "testuser"
                    }
                },
                "vcs": {
                    "commit": {
                        "subject": "Fix all the things"
                    }
                }
            }
            """;

        var pipeline = JsonSerializer.Deserialize<CircleCIPipeline>(json);

        Assert.That(pipeline, Is.Not.Null);
        Assert.That(pipeline!.Id, Is.EqualTo("abc-123"));
        Assert.That(pipeline.Number, Is.EqualTo(42));
        Assert.That(pipeline.Trigger?.Actor?.Login, Is.EqualTo("testuser"));
        Assert.That(pipeline.Vcs?.Commit?.Subject, Is.EqualTo("Fix all the things"));
    }

    [Test]
    public void CircleCIWorkflow_Deserialize_SetsProperties()
    {
        var json = """
            {
                "id": "wf-456",
                "name": "build-and-test",
                "status": "success",
                "pipeline_number": 42,
                "project_slug": "github/myorg/myrepo"
            }
            """;

        var workflow = JsonSerializer.Deserialize<CircleCIWorkflow>(json);

        Assert.That(workflow, Is.Not.Null);
        Assert.That(workflow!.Id, Is.EqualTo("wf-456"));
        Assert.That(workflow.Name, Is.EqualTo("build-and-test"));
        Assert.That(workflow.Status, Is.EqualTo("success"));
        Assert.That(workflow.PipelineNumber, Is.EqualTo(42));
        Assert.That(workflow.ProjectSlug, Is.EqualTo("github/myorg/myrepo"));
    }

    [Test]
    public void CircleCIPipelineResponse_Deserialize_SetsItems()
    {
        var json = """
            {
                "items": [
                    {
                        "id": "p-001",
                        "number": 1,
                        "updated_at": "2024-01-15T10:00:00Z"
                    },
                    {
                        "id": "p-002",
                        "number": 2,
                        "updated_at": "2024-01-14T09:00:00Z"
                    }
                ]
            }
            """;

        var response = JsonSerializer.Deserialize<CircleCIPipelineResponse>(json);

        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Items, Has.Count.EqualTo(2));
        Assert.That(response.Items![0].Id, Is.EqualTo("p-001"));
        Assert.That(response.Items![1].Number, Is.EqualTo(2));
    }

    [Test]
    public void CircleCIWorkflowResponse_Deserialize_SetsItems()
    {
        var json = """
            {
                "items": [
                    {
                        "id": "wf-001",
                        "name": "ci",
                        "status": "failed",
                        "pipeline_number": 10,
                        "project_slug": "github/org/repo"
                    }
                ]
            }
            """;

        var response = JsonSerializer.Deserialize<CircleCIWorkflowResponse>(json);

        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Items, Has.Count.EqualTo(1));
        Assert.That(response.Items![0].Status, Is.EqualTo("failed"));
    }

    [Test]
    public void CircleCIWorkflow_Pipeline_NotDeserializedFromJson()
    {
        var json = """
            {
                "id": "wf-789",
                "name": "deploy",
                "status": "running",
                "pipeline_number": 99,
                "project_slug": "github/org/repo",
                "Pipeline": { "id": "should-be-ignored", "number": 1 }
            }
            """;

        var workflow = JsonSerializer.Deserialize<CircleCIWorkflow>(json);

        Assert.That(workflow, Is.Not.Null);
        // Pipeline property is [JsonIgnore], so it should not be deserialized
        Assert.That(workflow!.Pipeline, Is.Null);
    }
}
